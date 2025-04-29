#include <iostream>
#include <unordered_set>

using namespace std;

string preprocess(const string& s) {
    string result = "#";
    for (char c : s) {
        result += c;
        result += "#";
    }
    return result;
}

unordered_set<string> manaker_pls_find_palindromic_substrings(const string& s) {
    string T = preprocess(s);
    int n = T.size();
    vector<int> P(n, 0);
    int C = 0, R = 0;

    for (int i = 1; i < n - 1; i++) {
        int mirror = 2 * C - i; // i' = C - (i - C)

        if (R > i) {
            P[i] = min(R - i, P[mirror]);
        }

        while (T[i + P[i] + 1] == T[i - P[i] - 1]) {
            P[i]++;
        }

        if (i + P[i] > R) {
            C = i;
            R = i + P[i];
        }
    }

    unordered_set<string> palindromes;
    for (int i = 1; i < n - 1; i++) {
        if (P[i] > 0) {
            string palindrome = s.substr((i - P[i]) / 2, P[i]);
            palindromes.insert(palindrome);
        }
    }

    return palindromes;
}

unordered_set<string> find_palindromic_substrings(const string& s) {
    unordered_set<string> palindromes;
    size_t n = s.length();
    for (size_t i = 0; i < s.length(); ++i) {
        size_t l = i, r = i;

        while (l >= 0 && r < n) {
            if (s[l] == s[r]) {
                palindromes.insert(s.substr(l, r - l + 1));
            }
            --l;
            ++r;
        }

        l = i, r = i + 1;
        while (l >= 0 && r < n) {
            if (s[l] == s[r]) {
                palindromes.insert(s.substr(l, r - l + 1));
            }
            --l;
            ++r;
        }
    }
    return palindromes;
}

int main(int argc, char* argv[]) {
    bool show_all = false;
    if (argc > 1 && string(argv[1]) == "--all") {
        show_all = true;
    }

    string input;
    cout << "> ";
    cin >> input;

    unordered_set<string> palindromes = manaker_pls_find_palindromic_substrings(input);
    for (const string& palindrome : palindromes) {
        if (palindrome.length() == 1 && !show_all) {
            continue;
        }
        cout << palindrome << endl;
    }
    return 0;
}
