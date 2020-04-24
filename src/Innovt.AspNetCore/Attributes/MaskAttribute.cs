namespace Innovt.AspNetCore.Attributes
{
    //public enum MaskType
    //{
    //    Custom,
    //    CPF,
    //    CNPJ,
    //    DATE,
    //    TIME,
    //    MONEY,
    //    PHONE
    //}
    /////// <summary>
    /////// Use this attribute to mask you property without 
    /////// </summary>
    //public class MaskAttribute : Attribute, IClientModelValidator
    //{
    //    private string mask;
    //    private MaskType maskType = MaskType.Custom;

    //    /// <inheritdoc />
    //    ///  <summary>
    //    /// Choose the masktype to mask your property
    //    ///  </summary>
    //    ///  <param name="maskType"></param>
    //    ///  <param name="mask">If you want to override the default mask</param>
    //    public MaskAttribute(MaskType maskType, string mask = null)
    //    {
    //        this.maskType = maskType;

    //        this.mask = string.IsNullOrEmpty(mask) ? GetMaskExpression() : mask;
    //    }

    //    public void AddValidation(ClientModelValidationContext context)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException(nameof(context));
    //        }

    //        context.Attributes.Add("alt", maskType.ToString());
            
    //        //var mask = GetMaskExpression(maskType);

    //        //MergeAttribute(context.Attributes, "data-val", "true");
    //    }

    //    private string GetMaskExpression()
    //    {
    //        switch (maskType)
    //        {
    //            case MaskType.CPF:
    //                return "999.999.999-99";
    //            case MaskType.CNPJ:
    //                return "99.999.999/9999-99";
    //            case MaskType.DATE:
    //                return "39/19/9999";
    //            case MaskType.TIME:
    //                return "29:69";
    //            case MaskType.MONEY:
    //                return "99,999.999.999.999";
    //            case MaskType.PHONE:
    //                return "(99) 99999-9999";
    //            default:
    //                return String.Empty;
    //        }
    //    }
    //}
}