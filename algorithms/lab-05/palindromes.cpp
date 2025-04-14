#include <iostream>
#include <unordered_set>

using namespace std;

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

    unordered_set<string> palindromes = find_palindromic_substrings(input);
    for (const string& palindrome : palindromes) {
        if (palindrome.length() == 1 && !show_all) {
            continue;
        }
        cout << palindrome << endl;
    }
    return 0;
}
