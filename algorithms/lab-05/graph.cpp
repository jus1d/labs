#include <cstddef>
#include <iostream>
#include <vector>
#include <algorithm>

#define endl '\n'

using namespace std;

void find_cycles(int node, int start, int depth, const vector<vector<int>>& graph, vector<bool>& visited, vector<int>& path, vector<vector<int>>& cycles, bool directed) {
    visited[node] = true;
    path.push_back(node);

    for (int neighbor : graph[node]) {
        if (!directed && !path.empty() && neighbor == path[path.size()-2]) {
            continue;
        }

        if (!visited[neighbor]) {
            find_cycles(neighbor, start, depth + 1, graph, visited, path, cycles, directed);
        }
        else if (neighbor == start && path.size() >= 3) {
            vector<int> cycle(path.begin(), path.end());
            cycle.push_back(start);

            bool is_new = true;
            for (const auto& existing : cycles) {
                if (existing.size() == cycle.size() &&
                    is_permutation(existing.begin(), existing.end(), cycle.begin())) {
                    is_new = false;
                    break;
                }
            }

            if (is_new) {
                cycles.push_back(cycle);
            }
        }
    }

    visited[node] = false;
    path.pop_back();
}

vector<vector<int>> get_cycles(const vector<vector<int>>& graph, bool directed) {
    int n = graph.size();
    vector<vector<int>> cycles;
    vector<bool> visited(n, false);
    vector<int> path;

    for (int i = 0; i < n; ++i) {
        if (!directed && visited[i]) continue;

        find_cycles(i, i, 0, graph, visited, path, cycles, directed);
        visited[i] = true;
    }

    return cycles;
}

void print_cycles(const vector<vector<int>>& cycles) {
    cout << "Found " << cycles.size() << " cycle(s)" << endl;
    for (size_t i = 0; i < cycles.size(); ++i) {
        cout << "[";
        for (size_t j = 0; j < cycles[i].size(); ++j) {
            int node = cycles[i][j];
            if (j == cycles[i].size() - 1) cout << node;
            else cout << node << " -> ";
        }
        cout << "]" << endl;
    }
}

int main(int argc, char* argv[]) {
    bool verbose = false;
    if (argc > 1 && string(argv[1]) == "--verbose") {
        verbose = true;
    }

    int n, m;
    bool directed;

    if (verbose) cout << "Enter amount of vertices and edges: ";
    cin >> n >> m;

    if (verbose) cout << "Is graph directed? (0 - no, otherwise - yes): ";
    cin >> directed;

    vector<vector<int>> graph(n);

    if (verbose) cout << "Enter edges (pairs of vertices from 0 to " << n-1 << "):" << endl;
    for (int i = 0; i < m; ++i) {
        int u, v;
        cin >> u >> v;
        graph[u].push_back(v);
        if (!directed && u != v) {
            graph[v].push_back(u);
        }
    }

    vector<vector<int>> cycles = get_cycles(graph, directed);
    print_cycles(cycles);

    return 0;
}
