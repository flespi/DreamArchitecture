using System.Text.Json;
using CleanArchitecture.Domain.ValueObjects;
using HotChocolate.Features;
using HotChocolate.Language;
using HotChocolate.Text.Json;

namespace CleanArchitecture.Graph.Schema.Shared.Scalars;

public class ColourType : ScalarType<Colour, StringValueNode>
{
    public ColourType() : base(nameof(Colour))
    {
    }

    protected override Colour OnCoerceInputLiteral(StringValueNode valueLiteral)
        => new(valueLiteral.Value);

    protected override Colour OnCoerceInputValue(JsonElement inputValue, IFeatureProvider context)
        => new(inputValue.GetString()!);

    protected override void OnCoerceOutputValue(Colour runtimeValue, ResultElement resultValue)
        => resultValue.SetStringValue(runtimeValue);

    protected override StringValueNode OnValueToLiteral(Colour runtimeValue)
        => new(runtimeValue.ToString());
}
