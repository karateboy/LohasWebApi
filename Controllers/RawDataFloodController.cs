using LohasWebApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace LohasWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RawDataFloodController : ControllerBase
{
    private readonly ILogger<RawDataFloodController> _logger;
    private readonly RawDataFloodIo _RawDataFloodIo;
    public RawDataFloodController(ILogger<RawDataFloodController> logger, RawDataFloodIo RawDataFloodIo)
    {
        _logger = logger;
        _RawDataFloodIo = RawDataFloodIo;
    }

    [HttpGet(Name = "GetRawDataFlood")]
    public async Task<IEnumerable<RawDataFloodIo.RawDataFloodModel>> Get([FromQuery] string deviceSeq)
    {
        var ret = await _RawDataFloodIo.GetRawDataFloodAsync(DateTime.Now.AddYears(-3), DateTime.Now, deviceSeq);
        return ret;
    }


}