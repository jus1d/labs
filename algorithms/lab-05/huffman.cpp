#include <iostream>
#include <string>
#include <queue>
#include <unordered_map>

using namespace std;

#define endl '\n'

struct Node
{
	char ch;
	int freq;
	Node *l, *r;
};

Node* create_node(char ch, int freq, Node* l, Node* r)
{
	Node* node = new Node();

	node->ch = ch;
	node->freq = freq;
	node->l = l;
	node->r = r;

	return node;
}

struct comp
{
	bool operator()(Node* l, Node* r)
	{
		return l->freq > r->freq;
	}
};

void encode(Node* root, string str, unordered_map<char, string> &huffmanCode)
{
	if (root == nullptr)
		return;

	if (!root->l && !root->r) {
		huffmanCode[root->ch] = str;
	}

	encode(root->l, str + "0", huffmanCode);
	encode(root->r, str + "1", huffmanCode);
}

void decode(Node* root, int &index, string str)
{
	if (root == nullptr) {
		return;
	}

	if (!root->l && !root->r)
	{
		cout << root->ch;
		return;
	}

	index++;

	if (str[index] =='0')
		decode(root->l, index, str);
	else
		decode(root->r, index, str);
}

int main()
{
	string text = "Huffman coding is a data compression algorithm.";

	unordered_map<char, int> freq;
	for (char ch: text) {
		freq[ch]++;
	}

	priority_queue<Node*, vector<Node*>, comp> pq;

	for (auto pair: freq) {
		pq.push(create_node(pair.first, pair.second, nullptr, nullptr));
	}

	while (pq.size() != 1)
	{
		Node *l = pq.top(); pq.pop();
		Node *r = pq.top();	pq.pop();

		int sum = l->freq + r->freq;
		pq.push(create_node('\0', sum, l, r));
	}

	Node* root = pq.top();

	unordered_map<char, string> huffmanCode;
	encode(root, "", huffmanCode);


	cout << "Table of Huffman codes:" << endl;
	for (const auto& pair: huffmanCode) {
		cout << "'" << pair.first << "' => " << pair.second << endl;
	}

	cout << endl << "Original string:" << endl << text << endl;

	string str = "";
	for (char ch: text) {
		str += huffmanCode[ch];
	}

	cout << endl << "Encoded string is:" << endl << str << endl;

	int index = -1;
	cout << endl << "Decoded string:" << endl;
	while (index < (int)str.size() - 2) {
		decode(root, index, str);
	}

	return 0;
}
