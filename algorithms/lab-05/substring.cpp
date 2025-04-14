#include <iostream>

using namespace std;

bool has_substring(const string& s, const string& sub) {
    for (size_t i = 0; i <= s.size() - sub.size(); ++i) {
        bool ok = true;
        for (size_t j = 0; j < sub.size() && ok; ++j) {
            cout << "checking " << s[i + j] << " and " << sub[j] << endl;
            if (s[i + j] != sub[j]) ok = false;
        }

        if (ok) return true;
    }
    return false;
}

int main() {
    string s, sub;
    cin >> s >> sub;

    if (has_substring(s, sub)) {
        cout << "YES" << endl;
    } else {
        cout << "NO" << endl;
    }
}
