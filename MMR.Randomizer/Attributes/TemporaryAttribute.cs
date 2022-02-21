using MMR.Randomizer.Models.Settings;
using System;
using System.Linq.Expressions;

namespace MMR.Randomizer.Attributes
{
    public class TemporaryAttribute : Attribute
    {
        public Func<GameplaySettings, bool> Condition { get; }

        public TemporaryAttribute()
        {
            Condition = settings => true;
        }

        public TemporaryAttribute(string settingFlagProperty, int flagValue, bool hasFlag)
        {
            var parameter = Expression.Parameter(typeof(GameplaySettings));

            // settings => (((int)settings[settingFlagProperty] & flagValue) != 0) == hasFlag
            var flagExpression = Expression.Equal(
                Expression.NotEqual(
                    Expression.And(
                        Expression.Convert(Expression.Property(parameter, settingFlagProperty), typeof(int)),
                        Expression.Constant(flagValue)
                        ),
                    Expression.Constant(0)
                    ),
                Expression.Constant(hasFlag)
            );
            Condition = Expression.Lambda<Func<GameplaySettings, bool>>(flagExpression, parameter).Compile();
        }
    }
}
