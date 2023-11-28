namespace LohasWebApi.Database;

using Dapper;
public class RawDataAirIo
{
    private readonly ILogger<RawDataAirIo> _logger;
    private readonly DapperContext _context;
    public RawDataAirIo(ILogger<RawDataAirIo> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
    }

    public class RawDataAirModel
    {
        public required string DeviceSeq { get; set; }
        public required Guid Id { get; set; }
        public required string EventSeq { get; set; }
        public DateTime EventDate { get; set; }
        public int EventMinutes { get; set; }
        public double ValuePm1 { get; set; }
        public double ValuePm2Dot5 { get; set; }
        public double ValuePm10 { get; set; }
        public double ValueCo { get; set; }
        public double ValueCo2 { get; set; }
        public double ValueO3 { get; set; }
        public double ValueTvoc { get; set; }
        public double ValueHcho { get; set; }
        public double ValueFungi { get; set; }
        public double ValueTemperature { get; set; }
        public double ValueHumidity { get; set; }
        public double ValueIqa { get; set; }
        public double ValueMold { get; set; }
        public DateTime CreateTime { get; set; }

        // 新增屬性，用於在需要時格式化 CreateTime 為 "HH:mm"
        public string FormattedCreateTime => CreateTime.ToString("HH:mm");
    }

    public async Task<IEnumerable<RawDataAirModel>> GetRawDataAirAsync(DateTime start, DateTime end, string deviceSeq)
    {
        using var connection = _context.CreateConnection();
        /*var sql = @"
            SELECT TOP (10) *
            FROM [ktl].[RawDataAir]
            WHERE CreateTime BETWEEN @start AND @end";*/

        var sql = @"
            SELECT TOP (10) * 
            FROM [ktl].[RawDataAir_back]
            WHERE CreateTime BETWEEN @start AND @end";

        // Add condition for eventSeq if it is provided
        if (!string.IsNullOrEmpty(deviceSeq))
        {
            sql += " AND DeviceSeq = @deviceSeq";
        }

        sql += " ORDER BY CreateTime DESC";

        try
        {
            return await connection.QueryAsync<RawDataAirModel>(sql, new
            {
                start,
                end,
                deviceSeq
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetRawDataAirAsync Error");
            throw;
        }
    }
}