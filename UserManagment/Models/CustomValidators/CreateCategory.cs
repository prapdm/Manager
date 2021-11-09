using FluentValidation;
using Manager.Entities;
using System;
using System.IO;
using System.Linq;

namespace Manager.Models.CustomValidators
{
    public class CreateCategory : AbstractValidator<CategoryDto>
    {
        private readonly long _fileSizeLimit;
        private string[] permittedExtensions = { ".jpg", ".gif", ".png" };

        public CreateCategory(ManagerDbContext dBContext)
        {
            _fileSizeLimit = 2097152;

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Slug)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var InUse = dBContext.Categories.Any(u => u.Slug == value);
                    if (InUse)
                    {
                        if(context.InstanceToValidate.Update is false)
                        context.AddFailure("Slug", "That Slug is taken");
                    }

                });

            RuleFor(x => x.ImageFile)
                .Custom((value, context) =>
                {
                    if (value is not null)
                    {
                        if (value.Length > _fileSizeLimit)
                            context.AddFailure("Image", $"File size limit is {_fileSizeLimit} KB");

                        var ext = Path.GetExtension(value.FileName).ToLowerInvariant();
                        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                            context.AddFailure("Image", $"You can only upload {String.Join(", ", permittedExtensions)} ");
                    }

                });
        }
    }
}
