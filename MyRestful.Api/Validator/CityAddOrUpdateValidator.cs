using FluentValidation;
using MyRestful.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Api.Validator
{
    public class CityAddOrUpdateValidator<T> : AbstractValidator<T> where T : CityAddOrUpdateVM
    {
        public CityAddOrUpdateValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("名称").WithMessage("{PropertyName}是必填项")
                .MaximumLength(20).WithMessage("{PropertyName}的长度不能超过{MaxLength}")
                .NotEqual("中国").WithMessage("{PropertyName}的值不可以是{ComparisonValue}");

            RuleFor(m => m.Description)
                .MaximumLength(100).WithName("描述").WithMessage("{PropertyName}的长度不能超过{MaxLength}");

        }
    }
}
