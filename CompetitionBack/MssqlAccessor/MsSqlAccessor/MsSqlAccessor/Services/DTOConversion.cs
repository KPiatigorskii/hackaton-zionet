using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MsSqlAccessor.Models;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MsSqlAccessor.Services
{

    public static class ModelConverterExtensions
    {
        public static TDto ConvertToDto<T, TDto>(this T model) where T : class where TDto : class, new()
        {
            TDto modelDTO = new TDto();
            foreach (var property in typeof(TDto).GetProperties())
            {
                try
                {
                    property.SetValue(modelDTO, typeof(T).GetProperty(property.Name)?.GetValue(model));
                } 
                catch (Exception ex)
                {
                    //Exception конвертации типа
                }
            }
            return modelDTO;
        }

        public static T ConvertFromDto<T, TDto>(this TDto modelDTO) where T : class, new() where TDto : class
        {
            T model = new T();
            foreach (var property in typeof(TDto).GetProperties().Where(p => p.GetValue(modelDTO) != null))
            {
                try
                {
                    typeof(T).GetProperty(property.Name)?.SetValue(model, property.GetValue(modelDTO));
                }
                catch (Exception ex)
                {
                    //Exception конвертации типа
                }
            }
            return model;
        }

        public static T MakeChangesFromDto<T, TDto>(this T model, TDto modelDTO) where T : class, new() where TDto : class
        {
            foreach (var property in typeof(TDto).GetProperties().Where(p => p.GetValue(modelDTO) != null))
            {
                try
                {
                    typeof(T).GetProperty(property.Name)?.SetValue(model, property.GetValue(modelDTO));
                }
                catch (Exception ex)
                {
                    //Exception конвертации типа
                }
            }
            return model;
        }
    }
}

