#include <iostream>
#include <vector>
#include <string>

using namespace std;
#define endl '\n';

struct Item {
    string name;
    int weight;
    int price;

    Item(string n, int w, int p) : name(n), weight(w), price(p) {}
};

pair<int, vector<Item>> knapsack_no_repetitions(const vector<Item>& items, int capacity) {
    int n = items.size();
    vector<vector<int>> dp(n + 1, vector<int>(capacity + 1, 0));
    vector<vector<vector<Item>>> included(n + 1, vector<vector<Item>>(capacity + 1));

    for (int i = 1; i <= n; ++i) {
        for (int j = 1; j <= capacity; ++j) {
            if (items[i-1].weight > j) {
                dp[i][j] = dp[i-1][j];
                included[i][j] = included[i-1][j];
            } else {
                int with_item = items[i-1].price + dp[i-1][j-items[i-1].weight];
                if (with_item > dp[i-1][j]) {
                    dp[i][j] = with_item;
                    included[i][j] = included[i-1][j-items[i-1].weight];
                    included[i][j].push_back(items[i-1]);
                } else {
                    dp[i][j] = dp[i-1][j];
                    included[i][j] = included[i-1][j];
                }
            }
        }
    }
    return {dp[n][capacity], included[n][capacity]};
}

pair<int, vector<Item>> knapsack_with_repetitions(const vector<Item>& items, int capacity) {
    vector<int> max_cost(capacity + 1, 0);
    vector<vector<Item>> included(capacity + 1);

    for (int j = 1; j <= capacity; ++j) {
        for (size_t i = 0; i < items.size(); ++i) {
            if (items[i].weight <= j) {
                int current = items[i].price + max_cost[j-items[i].weight];
                if (current > max_cost[j]) {
                    max_cost[j] = current;
                    included[j] = included[j-items[i].weight];
                    included[j].push_back(items[i]);
                }
            }
        }
    }
    return {max_cost[capacity], included[capacity]};
}

void display(const string& title, int value, const vector<Item>& items) {
    cout << title << value << endl;
    cout << "Items: ";
    for (size_t i = 0; i < items.size(); ++i) {
        if (i != 0) cout << ", ";
        cout << items[i].name;
    }
    cout << endl;
}

int main() {
    vector<Item> items = {
        {"Laptop", 1, 1500},
        {"Crisps", 4, 3000},
        {"Tea", 3, 2000}
    };

    int capacity = 4;

    auto result = knapsack_no_repetitions(items, capacity);
    cout << "NO REPS" << endl;
    display("Max cost: ", result.first, result.second);

    cout << endl;

    result = knapsack_with_repetitions(items, capacity);
    cout << "NO REPS" << endl;
    display("Knapscak with reps: Max cost: ", result.first, result.second);

    return 0;
}
