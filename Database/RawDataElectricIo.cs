namespace LohasWebApi.Database;

using Dapper;
public class RawDataElectricIo
{
    private readonly ILogger<RawDataElectricIo> _logger;
    private readonly DapperContext _context;
    public RawDataElectricIo(ILogger<RawDataElectricIo> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
    }

    public class RawDataElectricModel
    {
        public long DeviceSeq { get; set; }
        public required Guid Id { get; set; }
        public required string EventSeq { get; set; }
        public DateTime EventDate { get; set; }
        public int EventMinutes { get; set; }
        public int ValueDem { get; set; }
        public double ValueKw { get; set; }
        public double ValueKwd { get; set; }
        public double ValueKwm { get; set; }
        public double ValueMdm { get; set; }
        public double ValueCap { get; set; }
        public DateTime CreateTime { get; set; }
    }
    
    public async Task<IEnumerable<RawDataElectricModel>> GetRawDataElectricAsync(DateTime start, DateTime end)
    {
        using var connection = _context.CreateConnection();
        var sql = @"
            SELECT TOP (10) *
            FROM [ktl].[RawDataElectric]
            WHERE CreateTime BETWEEN @start AND @end";
        try
        {
            return await connection.QueryAsync<RawDataElectricModel>(sql, new
            {
                start,
                end
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetRawDataElectricAsync Error");
            throw;
        }
    }
}