using FreeSqlTest;

Console.WriteLine("开始测试");

var connStr = @"Host=127.0.0.1;Port=8123;Username=root;Password=123;Database=Test";

var fsql = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.ClickHouse, connStr)
    .UseAutoSyncStructure(true)
    //.UseNoneCommandParameter(true)
    .UseExitAutoDisposePool(false)
    .Build();

fsql.Aop.CommandBefore += (_, e) =>
{

};
var id = fsql.Ado.Query<int>("select @id", new { id = 1 });



Console.WriteLine("开始创建数据");

var json = "[{\"date\":\"2021-12-19T02:47:53.4365075 08:00\",\"temperatureC\":6,\"temperatureF\":42,\"summary\":\"Balmy\"},{\"date\":\"2021-12-20T02:47:53.4366893 08:00\",\"temperatureC\":36,\"temperatureF\":96,\"summary\":\"Bracing\"},{\"date\":\"2021-12-21T02:47:53.4366903 08:00\",\"temperatureC\":-15,\"temperatureF\":6,\"summary\":\"Bracing\"},{\"date\":\"2021-12-22T02:47:53.4366904 08:00\",\"temperatureC\":14,\"temperatureF\":57,\"summary\":\"Cool\"},{\"date\":\"2021-12-23T02:47:53.4366905 08:00\",\"temperatureC\":29,\"temperatureF\":84,\"summary\":\"Mild\"}]";

var content = "是/否测试Test";

var data = new List<TestTable>();
for (int i = 0; i < 10; i++)
{
    var t = new TestTable
    {
        Id = Guid.NewGuid().ToString(),
        Content = content,
        Content2 = json,
        Time = DateTime.Now
    };

    Console.WriteLine("测试单个插入字符串值改变");
    await fsql.Insert(t).NoneParameter().ExecuteAffrowsAsync();

    data.Add(t);
}

Console.WriteLine("测试插入超长字符串");

json += json;
json += json;
json += json;
json += json;
json += json;
json += json;
json += json;
json += json;
json += json;
json += json;
json += json;
json += json;

data.First().Content2 = json;

//成功插入超长字符串
//await fsql.Insert(data).ExecuteAffrowsAsync();

var result = await fsql.Select<TestTable>().OrderByDescending(x => x.Time).Take(20).ToListAsync();
foreach (var res in result)
{
    Console.WriteLine(res);
}

//单个插入报错
await fsql.Insert(data.First()).NoneParameter().ExecuteAffrowsAsync();


Console.WriteLine("结束测试");