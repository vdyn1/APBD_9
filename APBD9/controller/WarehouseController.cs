using APBD_9.model.DTOs;
using APBD_9.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APBD_9.controller;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;


    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }


    [HttpPost]
    public async Task<IActionResult> AddProductToWarehouse([FromBody] AddProductToWarehouseRequest request)
    {
        try
        {
            var newId = await _warehouseService.AddProductToWarehouseAsync(request);
            return Ok(new { Id = newId });
        }
        catch (ServiceException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }


    // [HttpGet("ping")]
    // public IActionResult Ping() => Ok("alive");

    
    // [HttpPut("procedure")]
    // public async Task<IActionResult> AddProduct([FromBody] AddProductToWarehouseRequest request)
    // {
    //     try
    //     {
    //         var newId = await _warehouseService.AddProductToWarehouseProcedure(request);
    //         return Ok(new { Id = newId });
    //     }
    //     catch (SqlException ex)
    //     {
    //         return StatusCode(500, $"[SQL ERROR] {ex.Message}");
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"[ERROR] {ex.Message}");
    //     }
    // }
    
    
    [HttpPut("procedure")]
    public async Task<IActionResult> AddProductUsingProcedure([FromBody] AddProductToWarehouseRequest request)
    {
        try
        {
            var id = await _warehouseService.AddProductToWarehouseProcedure(request);
            return Ok(new { IdProductWarehouse = id });
        }
        catch (ServiceException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

}