import { useEffect, useState, useCallback } from 'react'
import api from './services/api'

const clienteIdConsultado = '3b14a17a-ccfd-477a-8794-c9e0e501fb45'

function App() {
  const [pendentes, setPendentes] = useState([])
  const [posicao, setPosicao] = useState(null)
  const [extrato, setExtrato] = useState([])
  const [posicaoGlobal, setPosicaoGlobal] = useState([])

  const carregarDadosDoSistema = useCallback(async () => {
    try {
      const [resPendentes, resPosicao, resExtrato, resGlobal] = await Promise.all([
        api.get('/Operacoes/pendentes'),
        api.get(`/Operacoes/posicao/${clienteIdConsultado}`),
        api.get(`/Operacoes/cliente/${clienteIdConsultado}`),
        api.get('/Operacoes/posicao-global')
      ])

      setPendentes(resPendentes.data)
      setPosicao(resPosicao.data)
      setExtrato(resExtrato.data)
      setPosicaoGlobal(resGlobal.data)
    } catch (err) {
      console.error('Erro ao sincronizar dados:', err)
    }
  }, [])

  const confirmarPagamento = async (id) => {
    try {
      await api.post(`/Operacoes/${id}/processar`)
      await carregarDadosDoSistema()
    } catch (err) {
      console.error('Erro ao processar:', err)
      alert('Erro ao processar pagamento.')
    }
  }

  const gerarDadosDeTeste = async () => {
    try {
      const response = await api.post('/Operacoes/seed-data');
      console.log('Sucesso:', response.data);

      await carregarDadosDoSistema();
      alert('Dados de teste gerados com sucesso!');
    } catch (err) {
      const mensagemErro = err.response?.data?.erro || err.message;
      console.error('Erro detalhado:', err.response);
      alert('Erro ao gerar dados: ' + mensagemErro);
    }
  };

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      void carregarDadosDoSistema()
    }, 0)

    return () => clearTimeout(timeoutId)
  }, [carregarDadosDoSistema])

  return (
    <div style={{ padding: '0', fontFamily: 'Arial, sans-serif', backgroundColor: '#fcfcfc', minHeight: '100vh', color: '#333' }}>
      <header style={{ backgroundColor: '#fff', borderBottom: '3px solid #0056b3', padding: '15px 30px', display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: '16px' }}>
        <h1 style={{ margin: '0', fontSize: '1.4em', color: '#0056b3' }}>Motor de Liquidação Financeira</h1>
        <button
          onClick={gerarDadosDeTeste}
          style={{ backgroundColor: '#fff', color: '#0056b3', border: '1px solid #0056b3', padding: '8px 12px', borderRadius: '4px', cursor: 'pointer', fontSize: '0.9em' }}
        >
          Gerar Dados de Teste
        </button>
      </header>

      <div style={{ padding: '30px' }}>

        {/* EXPOSIÇÃO GLOBAL */}
        <section style={{ backgroundColor: '#f1f4f8', padding: '15px', marginBottom: '30px', border: '1px solid #d1d9e6' }}>
          <h3 style={{ margin: '0 0 10px 0', fontSize: '0.8em', color: '#555', textTransform: 'uppercase', letterSpacing: '1px' }}>Exposição Global de Ativos</h3>
          <div style={{ display: 'flex', gap: '20px' }}>
            {posicaoGlobal.length === 0 ? <small>Nenhum ativo liquidado no sistema.</small> : posicaoGlobal.map((g, i) => (
              <div key={i} style={{ background: '#fff', padding: '10px 20px', border: '1px solid #dee2e6', minWidth: '120px' }}>
                <small style={{ color: '#0056b3', fontWeight: 'bold' }}>{g.codigoAtivo}</small>
                <div style={{ fontSize: '1.2em', fontWeight: 'bold' }}>{g.quantidadeTotal} <span style={{ fontSize: '0.6em', color: '#888' }}>unid.</span></div>
              </div>
            ))}
          </div>
        </section>

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1.3fr', gap: '30px' }}>

          {/* COLUNA ESQUERDA */}
          <div>
            <section style={{ backgroundColor: '#fff', padding: '20px', borderLeft: '4px solid #e67e22', boxShadow: '0 2px 4px rgba(0,0,0,0.05)' }}>
              <h2 style={{ marginTop: 0, fontSize: '1.1em', borderBottom: '1px solid #eee', paddingBottom: '10px' }}>Aguardando Liquidação</h2>
              {pendentes.length === 0 ? <p style={{ fontSize: '0.9em' }}>Tudo liquidado.</p> : pendentes.map(op => (
                <div key={op.id} style={{ borderBottom: '1px solid #eee', padding: '10px 0', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                  <div>
                    <strong>{op.codigoAtivo}</strong> <br />
                    <small style={{ color: '#888' }}>Venc: {new Date(op.liquidacao.dataPrevista).toLocaleDateString('pt-BR')}</small>
                  </div>
                  <button onClick={() => confirmarPagamento(op.id)} style={{ background: '#0056b3', color: 'white', border: 'none', padding: '5px 10px', borderRadius: '3px', cursor: 'pointer' }}>Pagar</button>
                </div>
              ))}
            </section>

            <section style={{ marginTop: '20px', backgroundColor: '#fff', padding: '20px', borderLeft: '4px solid #2980b9', boxShadow: '0 2px 4px rgba(0,0,0,0.05)' }}>
              <h2 style={{ marginTop: 0, fontSize: '1.1em', borderBottom: '1px solid #eee', paddingBottom: '10px' }}>Sua Carteira</h2>
              {posicao ? (
                <table style={{ width: '100%', fontSize: '0.9em', marginTop: '10px' }}>
                  <thead><tr style={{ textAlign: 'left', color: '#888' }}><th>Ativo</th><th>Qtd</th><th style={{ textAlign: 'right' }}>Total</th></tr></thead>
                  <tbody>
                    {posicao.posicoes.map((p, i) => (
                      <tr key={i}><td style={{ padding: '5px 0' }}>{p.codigoAtivo}</td><td>{p.quantidadeTotal}</td><td style={{ textAlign: 'right' }}>R$ {p.valorTotalInvestido.toLocaleString('pt-BR')}</td></tr>
                    ))}
                  </tbody>
                </table>
              ) : <p>Carregando...</p>}
            </section>
          </div>

          {/* COLUNA DIREITA */}
          <section style={{ backgroundColor: '#fff', padding: '20px', borderLeft: '4px solid #2c3e50', boxShadow: '0 2px 4px rgba(0,0,0,0.05)' }}>
            <h2 style={{ marginTop: 0, fontSize: '1.1em', borderBottom: '1px solid #eee', paddingBottom: '10px' }}>Extrato de Operações</h2>
            <div style={{ maxHeight: '500px', overflowY: 'auto' }}>
              {extrato.map(op => {
                const dataBruta = op.executadaEm || op.dataOperacao;
                const dataFormatada = dataBruta ? new Date(dataBruta).toLocaleDateString('pt-BR') : 'Sem data';

                return (
                  <div key={op.id} style={{ padding: '10px 0', borderBottom: '1px solid #f9f9f9', display: 'flex', justifyContent: 'space-between' }}>
                    <div>
                      <strong style={{ color: '#0056b3' }}>{op.codigoAtivo}</strong> <br />
                      <small style={{ color: '#999' }}>{dataFormatada}</small>
                    </div>
                    <div style={{ textAlign: 'right' }}>
                      <span style={{ fontSize: '0.7em', fontWeight: 'bold', color: op.status === 1 ? '#155724' : '#856404' }}>
                        {op.status === 1 ? 'LIQUIDADA' : 'PENDENTE'}
                      </span> <br />
                      <span style={{ fontWeight: 'bold' }}>R$ {op.valorTotal.toLocaleString('pt-BR')}</span>
                    </div>
                  </div>
                );
              })}
            </div>
          </section>
        </div>
      </div>
    </div>
  )
}

export default App