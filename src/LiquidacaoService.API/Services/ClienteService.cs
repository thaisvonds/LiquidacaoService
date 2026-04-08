using LiquidacaoService.API.Domain.Entities;
using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Repositories;

namespace LiquidacaoService.API.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ClienteResponse>> BuscarTodos()
    {
        var clientes = await _repository.BuscarTodos();
        return clientes.Select(c => ParaResponse(c));
    }

    public async Task<ClienteResponse?> BuscarPorId(Guid id)
    {
        var cliente = await _repository.BuscarPorId(id);
        if (cliente is null) return null;
        return ParaResponse(cliente);
    }

    public async Task<ClienteResponse> Criar(CriarClienteRequest request)
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Cpf = request.Cpf,
            Email = request.Email
        };

        await _repository.Adicionar(cliente);
        await _repository.Salvar();

        return ParaResponse(cliente);
    }

    private ClienteResponse ParaResponse(Cliente cliente)
    {
        return new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cpf = cliente.Cpf,
            Email = cliente.Email,
            Ativo = cliente.Ativo,
            CriadoEm = cliente.CriadoEm
        };
    }
}