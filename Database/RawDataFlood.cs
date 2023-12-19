namespace LohasWebApi.Database;

using Dapper;
public class RawDataFloodIo
{
    private readonly ILogger<RawDataFloodIo> _logger;
    private readonly DapperContext _context;
    public RawDataFloodIo(ILogger<RawDataFloodIo> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
    }

    public class RawDataFloodModel
    {
        public long DeviceSeq { get; set; }
        public required Guid Id { get; set; }
        public required string EventSeq { get; set; }
        public DateTime EventDate { get; set; }
        public int EventMinutes { get; set; }
        public double Value { get; set; }
        public DateTime CreateTime { get; set; }

        // 新增屬性，用於在需要時格式化 CreateTime 為 "HH:mm"
        public string FormattedCreateTime => CreateTime.ToString("HH:mm");
    }

    public async Task<IEnumerable<RawDataFloodModel>> GetRawDataFloodAsync(DateTime start, DateTime end, string deviceSeq)
    {
        using var connection = _context.CreateConnection();
        /*var sql = @"
            SELECT TOP (10) *
            FROM [ktl].[RawDataFlood]
            WHERE CreateTime BETWEEN @start AND @end";*/

        var sql = @"
            SELECT TOP (10) * 
            FROM [ktl].[RawDataFlood_back]
            WHERE CreateTime BETWEEN @start AND @end";

        // Add condition for eventSeq if it is provided
        if (!string.IsNullOrEmpty(deviceSeq))
        {
            sql += " AND DeviceSeq = @deviceSeq";
        }

        sql += " ORDER BY CreateTime DESC";

        try
        {
            return await connection.QueryAsync<RawDataFloodModel>(sql, new
            {
                start,
                end,
                deviceSeq
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetRawDataFloodAsync Error");
            throw;
        }
    }
}