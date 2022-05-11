// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Cloud.Table;

public class BaseRequest
{
    public string IndexName { get; set; }

    public object Filter { get; set; }

    public string AttributesToGet { get; set; }
    public int? PageSize { get; set; }
    public string Page { get; set; }
    public string FilterExpression { get; set; }
}