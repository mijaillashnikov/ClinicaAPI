using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ClinicaAPI.Controllers;
using ClinicaAPI.Data;
using ClinicaAPI.Models;

namespace ClinicaAPI.Tests;

public class PacienteControllerTests
{
    private static ClinicaContext ConfigurarEntornoVirtual()
    {
        var parametros = new DbContextOptionsBuilder<ClinicaContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ClinicaContext(parametros);
    }

    [Fact]
    public void Post_AlIngresarNuevoRegistro_DevuelveConfirmacionExitosa()
    {
        var bdMemoria = ConfigurarEntornoVirtual();
        var controladorApi = new PacientesController(bdMemoria);
        var individuoPrueba = new Paciente
        {
            Nombre = "Lucia Ramos",
            Edad = 28,
            Diagnostico = "Examen de rutina",
            Telefono = "912345678",
            Direccion = "Calle Luna 45",
            CorreoElectronico = "lramos@correo.com",
            FechaRegistro = DateTime.UtcNow
        };

        var peticionResultante = controladorApi.Post(individuoPrueba);

        Assert.IsType<OkObjectResult>(peticionResultante);
        Assert.Single(bdMemoria.Pacientes);
    }

    [Fact]
    public void Get_AlSolicitarInventario_DevuelveListaConElementos()
    {
        var bdMemoria = ConfigurarEntornoVirtual();
        bdMemoria.Pacientes.Add(new Paciente { Nombre = "Marcos", Edad = 40, Diagnostico = "Revisión" });
        bdMemoria.SaveChanges();
        var controladorApi = new PacientesController(bdMemoria);

        var respuestaObtenida = controladorApi.Get();

        var confirmacion = Assert.IsType<OkObjectResult>(respuestaObtenida);
        var coleccionExtraida = Assert.IsAssignableFrom<IEnumerable<Paciente>>(confirmacion.Value);
        Assert.NotEmpty(coleccionExtraida);
    }
}