﻿using Cadastro.Auth.Application;
using CadastroApi.Controllers;
using Cadastro.Auth.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Api.UsuarioTests;

public class AdicionarUsuarioTests
{
    private readonly Mock<IMediator> _mediatorMock;    
    private readonly UsuarioController _controller;

    public AdicionarUsuarioTests()
    {
        _mediatorMock = new Mock<IMediator>();     
        _controller = new UsuarioController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AdicionarUsuario_InformadoDadosValidos_DeverRetornarOk()
    {
        var usuarioId = Guid.NewGuid();
        var command = new AdicionarUsuarioCommand
        {
            Nome = "novouser",
            Senha = "novasenha",
            Permissao = TipoUsuarioPermissao.Admin
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AdicionarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(usuarioId);

        var result = await _controller.AdicionarUsuario(command);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(usuarioId, createdResult.Value);
    }

    [Fact]
    public async Task AdicionarUsuario_InformadoDadosInvalidos_DeveRetornarBadRequest()
    {
        var usuarioId = Guid.NewGuid();
        var command = new AdicionarUsuarioCommand
        {
            Nome = "",
            Senha = "",
            Permissao = null
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AdicionarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new ValidationException(""));

        var result = await _controller.AdicionarUsuario(command);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
