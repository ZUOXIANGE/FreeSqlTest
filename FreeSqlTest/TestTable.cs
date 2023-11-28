using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace FreeSqlTest;

internal class TestTable
{
    [Required]
    [Column(IsIdentity = true)]
    public string Id { get; set; }

    [Column(StringLength = -2)]
    public string Content { get; set; }

    [Column(StringLength = -2)]
    public string Content2 { get; set; }

    [Column(DbType = "DateTime64(3, 'Asia/Shanghai')")]
    public DateTime Time { get; set; }

    public override string ToString()
    {
        return $"Id:{Id}  Content:{Content}  Content2:{Content2}  Time:{Time}";
    }
}