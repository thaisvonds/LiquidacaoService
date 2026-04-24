# LiquidacaoService — Motor de Liquidação Financeira

API desenvolvida em **.NET 10** com frontend em **React**, para gerenciar o 
ciclo de vida de operações financeiras (ações, FIIs e Renda Fixa), com foco 
no cálculo preciso de datas de liquidação (D+N) em dias úteis, respeitando 
feriados bancários e regras da B3.

## Contexto

Quando você compra uma ação na corretora, o dinheiro não sai da conta 
instantaneamente. A B3 define prazos específicos — D+2 para Renda Variável 
e D+1 para Renda Fixa. Este sistema atua como o motor que controla esses 
prazos e gerencia o processamento das liquidações.

Projeto desenvolvido para estudo prático de C#, .NET 10, Entity Framework 
Core e PostgreSQL, simulando um sistema real de uma squad de Liquidação e 
Informes de uma instituição financeira.

## 🛠 Tecnologias e Pilares de Qualidade

### **Backend (Motor de Liquidação)**
* **Runtime:** .NET 10 com C# (Long Term Support).
* **Persistência:** PostgreSQL 16 utilizando **Entity Framework Core 10** e Npgsql.
* **Observabilidade:** **Serilog** configurado para logs estruturados com escrita em Console e arquivos diários (`.txt`).
* **Resiliência:** Middleware Global de Exceções para tratamento de erros e padronização de respostas (Problem Details).
* **Documentação:** **Scalar** (Interface moderna e interativa para OpenAPI) disponível na rota `/scalar/v1`.

### **Frontend (Painel de Controle)**
* **Stack:** **React** com **Vite** para alta performance no desenvolvimento.
* **Comunicação:** **Axios** para integração assíncrona com a API REST.
* **UI/UX:** Dashboard responsivo desenvolvido para facilitar a gestão de ativos e liquidações pendentes.

### **Infraestrutura e QA**
* **Containerização:** **Docker** e **Docker Compose** para orquestração completa do ecossistema (API + Banco + Front).
* **Testes Unitários:** Suite de testes com **xUnit** focada na validação crítica da lógica de dias úteis e prazos de liquidação (D+1, D+2).
* **Arquitetura:** Aplicação de princípios de **Clean Architecture**, Injeção de Dependência e isolamento de camadas.

## Regras de Negócio

- Renda Variável (Ações e FIIs) liquida em **D+2 úteis**
- Renda Fixa liquida em **D+1 útil**
- Finais de semana e feriados nacionais são desconsiderados no cálculo
- Toda operação nasce com status `PENDENTE` e pode ser processada para `LIQUIDADA`
- Dados iniciais (ativos PETR4, BBAS3, MXRF11, HGLG11, Tesouro Selic, Tesouro IPCA 
  e feriados de 2026) são populados automaticamente via Seeder na primeira execução


## Arquitetura do Backend

Organizado em camadas com separação clara de responsabilidades:
Controllers/ → entrada HTTP Services/ → regras de negócio e cálculos de D+N Repositories/ → acesso ao banco de dados Domain/ → entidades e enums Data/ → DbContext e Seeder DTOs/ → objetos de entrada e saída da API


## Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /api/Operacoes | Registrar nova operação |
| GET | /api/Operacoes | Listar todas as operações |
| GET | /api/Operacoes/pendentes | Listar liquidações pendentes |
| GET | /api/Operacoes/cliente/{id} | Extrato de operações por cliente |
| GET | /api/Operacoes/posicao/{clienteId} | Posição consolidada por cliente |
| GET | /api/Operacoes/posicao-global | Exposição global de ativos |
| POST | /api/Operacoes/{id}/processar | Processar liquidação |
| GET | /api/Clientes | Listar clientes |
| POST | /api/Clientes | Cadastrar cliente |
| GET | /api/Ativos | Listar ativos |
| POST | /api/Ativos | Cadastrar ativo |


## Interface (Frontend)

O frontend exibe em tempo real:

- **Exposição Global de Ativos** — quantidade total liquidada por ativo no sistema
- **Aguardando Liquidação** — operações pendentes com botão de processamento
- **Sua Carteira** — posição consolidada do cliente com preço médio e total investido
- **Extrato de Operações** — histórico completo com status e valores

## Como Executar

### Pré-requisitos
- Docker Desktop

### Passo a passo

1. Clone o repositório:
```bash
git clone https://github.com/thaisvonds/LiquidacaoService.git
cd LiquidacaoService
```

2. Suba a aplicação completa com Docker:
```bash
docker compose up --build
```

3. Acesse a documentação interativa da API:
http://localhost:8080/scalar/v1

4. Acesse o frontend:
http://localhost:5173

> O banco de dados e a API sobem juntos automaticamente.  
> O Seeder popula os dados iniciais na primeira execução.

---

## Próximos Passos & Dívida Técnica

Embora funcional e robusto para o cenário atual, este projeto foi desenvolvido para fins de estudo e possui pontos de melhoria mapeados para versões futuras:

### **Roadmap de Evolução**
- [ ] **Autenticação e Autorização:** Implementação de Identity com JWT para remover o `ClientId` fixo do frontend e garantir que um cliente não veja as operações de outro.
- [ ] **Processamento Automático (Jobs):** Implementação de um *Background Service* (Worker) para processar as liquidações pendentes automaticamente ao final de cada dia útil, sem intervenção manual.
- [ ] **Persistência de Logs:** Configuração de volumes no Docker para que os arquivos do Serilog sejam persistidos permanentemente no host.
- [ ] **Notificações em Tempo Real:** Uso de **SignalR** para atualizar o dashboard do frontend assim que uma operação for liquidada pelo motor.

### **Melhorias de Código (Refactoring)**
- **Mapeamento de Enums:** Migrar a lógica de status de números mágicos (`0`, `1`) para um mapeamento de strings ou objetos no Frontend, evitando quebras caso o contrato do Backend mude.
- **Cache de Feriados:** Implementar **IDistributedCache** (Redis) para a consulta de feriados, evitando idas excessivas ao banco de dados em cálculos de datas em massa.
- **Frontend Errors:** Implementar uma camada de interceptors no Axios para capturar erros 500 e exibir alertas amigáveis ao usuário.

---

Desenvolvido por **Thais Cavalcante** como projeto de portfólio em C# e .NET,  
simulando sistemas críticos do mercado financeiro brasileiro.