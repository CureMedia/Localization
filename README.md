# Localization
Trying to make route data culture localization easier to setup for projects with or without MVC.

# Status
| Branch | Status |
|--------|--------|
| master | [![Build status](https://ci.appveyor.com/api/projects/status/ewg0xss3l681ram3?svg=true)](https://ci.appveyor.com/project/curemedia/localization) |
| develop | [![Build status](https://ci.appveyor.com/api/projects/status/ewg0xss3l681ram3/branch/develop?svg=true)](https://ci.appveyor.com/project/curemedia/localization/branch/develop) |


# Usage

# Internals
The default implementation use `IRequestCultureFeature` to get the `RequestCulture` for current request. The `RequestCulture` is in turn decided based on the work done by
1. **`AcceptLanguageHeaderCultureProvider`** Deduce the culture from `Accept-Language`-header. If it fails move on to next
2. **`CookieCultureProvider`** Deduce the culture from default implementation. If it fails move on to next
3. **`RouteDataRequestCultureProvider`** Deduce the culture from Url. If it fails use the `RequestLocalizationOptions.DefaultRequestCulture` value.

# Culture template
Default is to take the value from `CultureInfo.Name` which is either e.g. `en` or `en-US` based on supported cultures.

## Localization.Routing

## Localization.Routing.Mvc

## Authors

* [Joakim Carselind](https://github.com/joacar)


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
