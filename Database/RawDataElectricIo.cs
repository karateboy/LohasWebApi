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

        // 新增屬性，用於在需要時格式化 CreateTime 為 "HH:mm"
        public string FormattedCreateTime => CreateTime.ToString("HH:mm");
    }
    
    public async Task<IEnumerable<RawDataElectricModel>> GetRawDataElectricAsync(DateTime start, DateTime end, string deviceSeq)
    {
        using var connection = _context.CreateConnection();
        /*var sql = @"
            SELECT TOP (10) *
            FROM [ktl].[RawDataElectric]
            WHERE CreateTime BETWEEN @start AND @end";*/

        var sql = @"
            SELECT TOP (10) * 
            FROM [ktl].[RawDataElectric_back]
            WHERE CreateTime BETWEEN @start AND @end";

        // Add condition for eventSeq if it is provided
        if (!string.IsNullOrEmpty(deviceSeq))
        {
            sql += " AND DeviceSeq = @deviceSeq";
        }

        sql += " ORDER BY CreateTime DESC";

        try
        {
            return await connection.QueryAsync<RawDataElectricModel>(sql, new
            {
                start,
                end,
                deviceSeq
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetRawDataElectricAsync Error");
            throw;
        }
    }
    
    public class RawDataElectricPlotModel
    {
        public DateTime DateTime { get; set; }
        public double ValueKwAvg { get; set; }
        public double ValueKwdAvg { get; set; }
    }
    // provide average value for different time interval
    public async Task<IEnumerable<RawDataElectricPlotModel>> GetRawDataElectricDailyAvgAsync(string deviceSeq, DateTime start, DateTime end)
    {
        using var connection = _context.CreateConnection();
        var sql = @"
            SELECT EventDate as DateTime, AVG(ValueKw) AS ValueKwAvg, AVG(ValueKwd) AS ValueKwdAvg
            FROM [ktl].[RawDataElectric]
            WHERE CreateTime BETWEEN @start AND @end and DeviceSeq = @deviceSeq
            GROUP BY EventDate;";
        try
        {
            return await connection.QueryAsync<RawDataElectricPlotModel>(sql, new
            {
                deviceSeq,
                start,
                end
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetRawDataElectricDailyAvgAsync Error");
            throw;
        }
    }
    
    public async Task<IEnumerable<RawDataElectricPlotModel>> GetRawDataElectricMonthlyAvgAsync(string deviceSeq, DateTime start, DateTime end)
    {
        using var connection = _context.CreateConnection();
        var sql = @"
            SELECT DateAdd(Month, DateDiff(Month, 0, EventDate), 0) as DateTime, AVG(ValueKw) AS ValueKwAvg, AVG(ValueKwd) AS ValueKwdAvg
            FROM [ktl].[RawDataElectric]
            WHERE CreateTime >= @start AND CreateTime < @end and DeviceSeq = @deviceSeq
            GROUP BY DateAdd(Month, DateDiff(Month, 0, EventDate), 0)";
        try
        {
            return await connection.QueryAsync<RawDataElectricPlotModel>(sql, new
            {
                deviceSeq,
                start,
                end
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetRawDataElectricMonthlyAvgAsync Error");
            throw;
        }
    }
}