#include <iostream>
#include <vector>

constexpr double PI = 3.14159265358979323846;

using namespace std;

static double cot(double x) {
    return 1 / tan(x);
}

static double calculate (double a, double b) {
	double result = 0;
	const int c = 21;

	_asm {
		finit
		
		// 21 * PI * sin(a) + cos(b) * tan(a) - cot(b)
		fldpi
		fild    c
		fmulp   st(1), st       // 21 * PI
							    
		fld     qword ptr[a]
		fsin                    // sin(a)
		fmulp   st(1), st       // 21 * PI * sin(a)
							    
		fld     qword ptr[b]
		fcos                    // cos(b)
							    
		fld     qword ptr[a]    
		fptan                   // tan(a)
		fdivp   st(1), st	    
							    
		fmulp   st(1), st       // cos(b) * tan(a)
							    
		faddp   st(1), st       // 21 * PI * sin(a) + cos(b) * tan(a)
							    
		fld     qword ptr[b]    
		fptan				    
		fdivrp  st(1), st       // cot(b)
							    
		fsubp   st(1), st       // 21 * PI * sin(a) + cos(b) * tan(a) - cot(b)
		
		// PI * (b * b - a)
		fld     qword ptr[b]    
		fld     qword ptr[b]    
		fmulp   st(1), st       // b * b
							    
		fld     qword ptr[a]    
		fsubp   st(1), st       // b * b - a
							    
		fldpi				    
		fmulp   st(1), st       // PI * (b * b - a)
		
		// PI * (b * b - a) / (-21 * a + b)
		fild    c			    
		fchs                    // -21
		fld     qword ptr[a]    
		fmulp   st(1), st       // -21 * a
		fld     qword ptr[b]    
		faddp   st(1), st       // -21 * a + b
							    
		fdivp   st(1), st       // PI * (b * b - a) / (-21 * a + b)

		faddp   st(1), st       // 21 * PI * sin(a) + cos(b) * tan(a) - cot(b) + PI * (b * b - a) / (-21 * a + b)

		fstp    result
	}
	return result;
}

int main() {
	while (true) {
		double a, b;
		cout << "Enter `a` value -> ";
		cin >> a;

		cout << "Enter `b` value -> ";
		cin >> b;

		if (cos(a) == 0 || sin(b) == 0 || (-21 * a + b) == 0) {
			cout << "Computation impossible. Division by zero found!\n\n";
			continue;
		}

		cout << endl << "Expression: X = 21 * PI * sin(a) + cos(b) * tan(a) - cot(b) + PI * (b * b - a) / (-21 * a + b)" << endl;
		cout << "With: a = " << a << ", b = " << b << endl;
		cout << endl;

		cout << "[ASM] X = " << calculate(a, b) << endl;
		cout << "[C++] X = " << 21 * PI * sin(a) + cos(b) * tan(a) - cot(b) + PI * (b * b - a) / (-21 * a + b) << endl;
	}

    return 0;
}