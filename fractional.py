from scipy.special import gamma, factorial
from matplotlib import pyplot as plt
import numpy

ALPHA = 0


def x(x):
    return 2*x


def x_sq(x):
    return x**2


def dfx(alpha, f, x, h=1):
    res = 0

    def calc_term(n):
        a = f(x - n*h)
        b = factorial(n)
        term = alpha - n + 1
        c = gamma(term)

        return a/(b*c)
    sign = 1
    for i in range(100):
        res += sign * calc_term(i)
        sign *= -1
    res *= gamma(ALPHA + 1)
    return res


fig = plt.figure()
ax = numpy.linspace(-100, 100)

MAX_D = 1
GRAPHS = 10
arrs = []
for num in range(GRAPHS ):
    arrs.append([])

for num in ax:
    for i in range(GRAPHS):
        d = 0 + (i * MAX_D / GRAPHS)
        arrs[i].append(dfx(d, x_sq, num))

for num in arrs:
    plt.plot(ax, num)

plt.show()
