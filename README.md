# Localization
Trying to make route data culture localization easier to configure for ASP.NET Core and ASP.NET Core MVC.

# Status
| Branch | Status | Packages |
|--------|--------|-------|
| master | [![Build status](https://ci.appveyor.com/api/projects/status/ewg0xss3l681ram3?svg=true)](https://ci.appveyor.com/project/curemedia/localization) | [Localization.Routing](https://www.nuget.org/packages/Cure.AspNetCore.Localization.Routing/) [Localization.Routing.Mvc](https://www.nuget.org/packages/Cure.AspNetCore.Localization.Routing.Mvc/) |
| develop | [![Build status](https://ci.appveyor.com/api/projects/status/ewg0xss3l681ram3/branch/develop?svg=true)](https://ci.appveyor.com/project/curemedia/localization/branch/develop) | [MyGet](https://www.myget.org/feed/Packages/curemedia-ci) |


# Usage

## Install


# Internals
The default implementation use `IRequestCultureFeature` to get the `RequestCulture` for current request. The `RequestCulture` is in turn decided based on the work done by (in order)
1. `RouteDataRequestCultureProvider`
2. `CookieCultureProvider`
3. `AcceptLanguageHeaderCultureProvider`
4. `RequestLocalizationOptions.DefaultRequestCulture`

What it means is that if a user browse to your site for the first time, without any culture, it will be decuded by inspecting the `Accept-Language` header value(s) (step 3.). Once on your site presented with the option to select language (which creates the cookie), and does so, the next time this value will be used (step 2.).

# Culture template
Default is to take the value from `CultureInfo.Name` which is either e.g. `en` or `en-US` based on supported cultures.

## Localization.Routing

## Localization.Routing.Mvc

## Authors

* [Joakim Carselind](https://github.com/joacar)


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
