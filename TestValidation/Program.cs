using Finova.Services;using Finova.Services;

using Finova.Countries.Europe.Germany.Validators;using Finova.Countries.Europe.Germany.Validators;



Console.WriteLine("Testing German Steuernummer validation:");Console.WriteLine(""Testing German Steuernummer validation:"");

Console.WriteLine();Console.WriteLine();



// Direct validator tests// Direct validator tests

var testCases = new[] { "12345", "123456", "123456789012", "1234567890123", "12345678901234", "HRB 12345" };var testCases = new[] { ""12345"", ""123456"", ""123456789012"", ""1234567890123"", ""12345678901234"", ""HRB 12345"" };



foreach (var test in testCases)foreach (var test in testCases)

{{

    var result = GermanySteuernummerValidator.ValidateSteuernummer(test);    var result = GermanySteuernummerValidator.ValidateSteuernummer(test);

    var countryResult = EuropeEnterpriseValidator.ValidateEnterpriseNumber(test, "DE");    var countryResult = EuropeEnterpriseValidator.ValidateEnterpriseNumber(test, ""DE"");

    Console.WriteLine($"Input: '{test}'");    Console.WriteLine($""Input: '{test}'"");

    Console.WriteLine($"  Direct Validator: IsValid={result.IsValid}");    Console.WriteLine($""  Direct Validator: IsValid={result.IsValid}"");

    Console.WriteLine($"  Country Dispatch:  IsValid={countryResult.IsValid}");    Console.WriteLine($""  Country Dispatch:  IsValid={countryResult.IsValid}"");

    Console.WriteLine();    Console.WriteLine();

}}

