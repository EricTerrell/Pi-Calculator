# Pi-Calculator
Compute 𝛑 to a ludicrously high precision!

# Requires

.net 9.0 + Avalonia UI. Developed with JetBrains Rider IDE.

# Example

![Screenshot 2025-03-22 102042](https://github.com/user-attachments/assets/75d81b10-19d5-4820-b204-3a265ac99821)

# Credits

Uses the [ExtendedNumerics](https://www.nuget.org/packages/ExtendedNumerics.BigDecimal/) BigDecimal class.

Uses these algorithms to calculate 𝛑:

* [Andrew Jennings'](http://ajennings.net/blog/a-million-digits-of-pi-in-9-lines-of-javascript.html) Javascript code
* [Cygnus Software's](https://www.cygnus-software.com/misc/pidigits.htm) technique
* The [Plouffe / Bellard](https://bellard.org/pi/pi.c) algorithm

# References

https://math.tools/numbers/pi/1000000

# Performance

This program uses a single CPU core.

It calculated 1,000,000 digits in ~20 minutes on an AMD Ryzen 7 PRO 7840U with 64 GB RAM.

The Plouffe / Bellard code could be easily parallelized, but since the Andrew 
Jennings code is so much faster, there's really no point.