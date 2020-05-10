using System;
using Mindscape.LightSpeed;

/// <summary>
/// 
/// </summary>
[Table("explik_coursepage", IdentityMethod = IdentityMethod.IdentityColumn)]
public class CoursePageEntity : Entity<int>
{
    [Column("courseid")]
    private int _courseid;

    [Column("pageid")]
    private int _pageid;

    [Column("order")]
    private int _order;

    public int CourseId
    {
        get
        {
            return _courseid;
        }
        set
        {
            Set<int>(ref _courseid, value);
        }
    }

    public int PageId
    {
        get
        {
            return _pageid;
        }
        set
        {
            Set<int>(ref _pageid, value);
        }
    }


    public int Order
    {
        get
        {
            return _order;
        }
        set
        {
            Set<int>(ref _order, value);
        }
    }
}
