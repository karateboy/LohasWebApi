using LohasWebApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace LohasWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RawDataRainController : ControllerBase
{
    private readonly ILogger<RawDataRainController> _logger;
    private readonly RawDataRainIo _RawDataRainIo;
    public RawDataRainController(ILogger<RawDataRainController> logger, RawDataRainIo RawDataRainIo)
    {
        _logger = logger;
        _RawDataRainIo = RawDataRainIo;
    }

    [HttpGet(Name = "GetRawDataRain")]
    public async Task<IEnumerable<RawDataRainIo.RawDataRainModel>> Get([FromQuery] string deviceSeq)
    {
        var ret = await _RawDataRainIo.GetRawDataRainAsync(DateTime.Now.AddYears(-3), DateTime.Now, deviceSeq);
        return ret;
    }


}