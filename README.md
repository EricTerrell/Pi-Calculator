# Pi-Calculator
Compute 𝛑 to a ludicrously high precision!

# Requires

.NET 9.0 + Avalonia UI. Developed with JetBrains Rider IDE.

# Example

![Screenshot 2025-03-22 102042](https://github.com/user-attachments/assets/75d81b10-19d5-4820-b204-3a265ac99821)

# Credits

Uses the [ExtendedNumerics](https://www.nuget.org/packages/ExtendedNumerics.BigDecimal/) BigDecimal class.

Uses these algorithms to calculate 𝛑:

* [Andrew Jennings'](http://ajennings.net/blog/a-million-digits-of-pi-in-9-lines-of-javascript.html) Javascript code
* [Cygnus Software's](https://www.cygnus-software.com/misc/pidigits.htm) technique
* The [Plouffe / Bellard](https://bellard.org/pi/pi.c) algorithm

# How to Build

Go to the "Pi Calculator\\Pi Calculator\\deploy" folder. Run "deploy-all-framework-dependent.ps1" or
"deploy-all-self-contained.ps1".

The framework dependent build scripts will create relatively small executables,
which will require that .NET 9 has already been installed.

The self-contained scripts will create relatively large
executables which will not require a previous .NET 9 installation.

# Developers

Pi Calculator was developed in the C# programming language, using [`Avalonia`](https://avaloniaui.net/platforms), which allows developers 
to create .NET UI apps for Windows, Linux, and OSX.

The app was developed with the [`JetBrains Rider`](https://www.jetbrains.com/rider/) IDE.

After building the app, use the scripts in the "deploy" folder to generate the executable and dependencies.

# References

https://math.tools/numbers/pi/1000000

# Performance

This program uses a single CPU core.

It calculated 1,000,000 digits in ~20 minutes on an AMD Ryzen 7 PRO 7840U with 64 GB RAM.

The Plouffe / Bellard code could be easily parallelized, but since the Andrew 
Jennings code is so much faster, there's really no point.