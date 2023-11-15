﻿using LohasWebApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace LohasWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RawDataElectricController : ControllerBase
{
    private readonly ILogger<RawDataElectricController> _logger;
    private readonly RawDataElectricIo _rawDataElectricIo;
    public RawDataElectricController(ILogger<RawDataElectricController> logger, RawDataElectricIo rawDataElectricIo)
    {
        _logger = logger;
        _rawDataElectricIo = rawDataElectricIo;
    }
    
    [HttpGet(Name = "GetRawDataElectric")]
    public async Task<IEnumerable<RawDataElectricIo.RawDataElectricModel>> Get()
    {
        var ret = await _rawDataElectricIo.GetRawDataElectricAsync(DateTime.Now.AddYears(-3), DateTime.Now);  
        return ret;
    }
    
    
}