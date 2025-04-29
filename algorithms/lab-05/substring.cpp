#include <chrono>
#include <iostream>
#include <vector>

using namespace std;
using namespace std::chrono;

bool search_substring_naive(const string& s, const string& sub) {
    for (size_t i = 0; i <= s.size() - sub.size(); ++i) {
        bool ok = true;
        for (size_t j = 0; j < sub.size() && ok; ++j) {
            if (s[i + j] != sub[j]) ok = false;
        }

        if (ok) return true;
    }
    return false;
}

vector<int> precompute_lps(const string& pattern) {
    int n = pattern.size();
    vector<int> lps(n, 0);
    int len = 0;

    for (int i = 1; i < n; ) {
        if (pattern[i] == pattern[len]) {
            len++;
            lps[i] = len;
            i++;
        } else {
            if (len != 0) {
                len = lps[len - 1];
            } else {
                lps[i] = 0;
                i++;
            }
        }
    }
    return lps;
}

bool search_substring_kmp(const string& text, const string& pattern) {
    int n = text.size();
    int m = pattern.size();

    if (m == 0) return true;
    if (n < m) return false;

    vector<int> lps = precompute_lps(pattern);
    int i = 0;
    int j = 0;

    while (i < n) {
        if (pattern[j] == text[i]) {
            i++;
            j++;

            if (j == m) {
                return true;
            }
        } else {
            if (j != 0) {
                j = lps[j - 1];
            } else {
                i++;
            }
        }
    }

    return false;
}

int main() {
    string s, sub;
    cin >> s >> sub;

    cout << "[kmp]: ";
    auto start = high_resolution_clock::now();
    if (search_substring_kmp(s, sub)) {
        cout << "substring exists" << endl;
    } else {
        cout << "substring not found" << endl;
    }
    auto stop = high_resolution_clock::now();
    auto duration = duration_cast<microseconds>(stop - start);
    cout << "[kmp]: took " << duration.count() << " microseconds" << endl;

    cout << "[naive]: ";
    start = high_resolution_clock::now();
    if (search_substring_naive(s, sub)) {
        cout << "substring exists" << endl;
    } else {
        cout << "substring not found" << endl;
    }
    stop = high_resolution_clock::now();
    duration = duration_cast<microseconds>(stop - start);
    cout << "[naive]: took " << duration.count() << " microseconds" << endl;
}
