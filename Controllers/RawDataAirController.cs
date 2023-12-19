using LohasWebApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace LohasWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RawDataAirController : ControllerBase
{
    private readonly ILogger<RawDataAirController> _logger;
    private readonly RawDataAirIo _RawDataAirIo;
    public RawDataAirController(ILogger<RawDataAirController> logger, RawDataAirIo RawDataAirIo)
    {
        _logger = logger;
        _RawDataAirIo = RawDataAirIo;
    }

    [HttpGet(Name = "GetRawDataAir")]
    public async Task<IEnumerable<RawDataAirIo.RawDataAirModel>> Get([FromQuery] string deviceSeq)
    {
        var ret = await _RawDataAirIo.GetRawDataAirAsync(DateTime.Now.AddYears(-3), DateTime.Now, deviceSeq);
        return ret;
    }


}