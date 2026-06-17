#!/usr/bin/env python3

import torch
import torch.nn as nn
import torch.optim as optim
from torch.utils.data import DataLoader, TensorDataset
import numpy as np
import matplotlib.pyplot as plt
from collections import Counter
import re

device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
print(f"Device: {device}")

batch_size = 32
embedding_dim = 100
hidden_dim = 256
output_dim = 1
n_layers = 2
bidirectional = True
dropout = 0.3
learning_rate = 0.001
epochs = 10

def download_imdb_reviews():
    try:
        from keras.datasets import imdb
        word_index = imdb.get_word_index()
        reverse_word_index = dict([(value, key) for (key, value) in word_index.items()])

        def decode_review(text):
            return ' '.join([reverse_word_index.get(i - 3, '?') for i in text])

        (x_train, y_train), (x_test, y_test) = imdb.load_data(num_words=10000)

        train_reviews = [decode_review(x) for x in x_train[:1000]]
        train_labels = y_train[:1000]
        test_reviews = [decode_review(x) for x in x_test[:500]]
        test_labels = y_test[:500]

        return train_reviews, train_labels, test_reviews, test_labels
    except:
        print("Using generated synthetic reviews (harder)")

        positive_words = ['amazing', 'excellent', 'great', 'good', 'wonderful', 'fantastic', 'awesome', 'perfect', 'brilliant', 'outstanding', 'superb', 'wonderful', 'delightful']
        negative_words = ['terrible', 'bad', 'awful', 'horrible', 'poor', 'disappointing', 'waste', 'boring', 'dull', 'worst', 'mediocre', 'tedious', 'unwatchable']
        neutral_words = ['movie', 'film', 'watch', 'story', 'character', 'scene', 'plot', 'acting', 'director', 'production', 'dialogue', 'cinematography', 'music', 'pacing']

        opposite_sentiment = {
            'positive': negative_words,
            'negative': positive_words
        }

        np.random.seed(42)
        train_reviews = []
        train_labels = []

        for i in range(1000):
            label = np.random.randint(0, 2)
            words_choice = positive_words if label == 1 else negative_words

            num_words = np.random.randint(50, 120)
            sentiment_count = np.random.randint(4, 10)
            neutral_count = num_words - sentiment_count

            review_words = np.random.choice(words_choice, size=sentiment_count, replace=True).tolist()
            review_words += np.random.choice(neutral_words, size=neutral_count, replace=True).tolist()

            if np.random.random() < 0.1:
                opposite_words = np.random.choice(opposite_sentiment['positive' if label == 0 else 'negative'],
                                                 size=1, replace=True).tolist()
                review_words.extend(opposite_words)

            np.random.shuffle(review_words)
            review = ' '.join(review_words)
            train_reviews.append(review)
            train_labels.append(label)

        test_reviews = []
        test_labels = []
        for i in range(500):
            label = np.random.randint(0, 2)
            words_choice = positive_words if label == 1 else negative_words

            num_words = np.random.randint(50, 120)
            sentiment_count = np.random.randint(4, 10)
            neutral_count = num_words - sentiment_count

            review_words = np.random.choice(words_choice, size=sentiment_count, replace=True).tolist()
            review_words += np.random.choice(neutral_words, size=neutral_count, replace=True).tolist()

            if np.random.random() < 0.1:
                opposite_words = np.random.choice(opposite_sentiment['positive' if label == 0 else 'negative'],
                                                 size=1, replace=True).tolist()
                review_words.extend(opposite_words)

            np.random.shuffle(review_words)
            review = ' '.join(review_words)
            test_reviews.append(review)
            test_labels.append(label)

        return train_reviews, np.array(train_labels), test_reviews, np.array(test_labels)

train_reviews, train_labels, test_reviews, test_labels = download_imdb_reviews()
print(f"Train samples: {len(train_reviews)}, Test samples: {len(test_reviews)}")
print(f"Sample review: {train_reviews[0][:100]}...")
print(f"Label: {train_labels[0]}")

def preprocess_text(text):
    text = text.lower()
    text = re.sub(r'<br />', ' ', text)
    text = re.sub(r'[^a-z\s]', '', text)
    text = ' '.join(text.split())
    return text

train_reviews = [preprocess_text(review) for review in train_reviews]
test_reviews = [preprocess_text(review) for review in test_reviews]

vocab = Counter()
for review in train_reviews:
    vocab.update(review.split())

word_to_idx = {'<pad>': 0, '<unk>': 1}
for idx, (word, _) in enumerate(vocab.most_common(10000 - 2), 2):
    word_to_idx[word] = idx

idx_to_word = {v: k for k, v in word_to_idx.items()}

print(f"Vocabulary size: {len(word_to_idx)}")
print(f"Most common words: {vocab.most_common(10)}")

def texts_to_sequences(texts, word_to_idx, max_len=100):
    sequences = []
    for text in texts:
        seq = [word_to_idx.get(word, 1) for word in text.split()]
        if len(seq) < max_len:
            seq += [0] * (max_len - len(seq))
        else:
            seq = seq[:max_len]
        sequences.append(seq)
    return np.array(sequences)

x_train = texts_to_sequences(train_reviews, word_to_idx, max_len=100)
x_test = texts_to_sequences(test_reviews, word_to_idx, max_len=100)
y_train = np.array(train_labels, dtype=np.float32)
y_test = np.array(test_labels, dtype=np.float32)

print(f"Train data shape: {x_train.shape}, {y_train.shape}")
print(f"Test data shape: {x_test.shape}, {y_test.shape}")

x_train_tensor = torch.LongTensor(x_train)
y_train_tensor = torch.FloatTensor(y_train)
x_test_tensor = torch.LongTensor(x_test)
y_test_tensor = torch.FloatTensor(y_test)

train_dataset = TensorDataset(x_train_tensor, y_train_tensor)
test_dataset = TensorDataset(x_test_tensor, y_test_tensor)

train_loader = DataLoader(train_dataset, batch_size=batch_size, shuffle=True)
test_loader = DataLoader(test_dataset, batch_size=batch_size, shuffle=False)

print(f"Train batches: {len(train_loader)}, Test batches: {len(test_loader)}")

class SentimentLSTM(nn.Module):
    def __init__(self, vocab_size, embedding_dim, hidden_dim, output_dim, n_layers, bidirectional, dropout):
        super(SentimentLSTM, self).__init__()

        self.embedding = nn.Embedding(vocab_size, embedding_dim, padding_idx=0)
        self.lstm = nn.LSTM(
            embedding_dim,
            hidden_dim,
            num_layers=n_layers,
            bidirectional=bidirectional,
            dropout=dropout,
            batch_first=True
        )

        self.fc1 = nn.Linear(hidden_dim * 2 if bidirectional else hidden_dim, 128)
        self.dropout = nn.Dropout(dropout)
        self.fc2 = nn.Linear(128, output_dim)

    def forward(self, text):
        embedded = self.dropout(self.embedding(text))
        _, (hidden, cell) = self.lstm(embedded)

        if self.lstm.bidirectional:
            hidden = torch.cat([hidden[-1], hidden[-2]], dim=1)
        else:
            hidden = hidden[-1]

        out = self.fc1(hidden)
        out = torch.relu(out)
        out = self.dropout(out)
        out = self.fc2(out)
        out = torch.sigmoid(out)

        return out

model = SentimentLSTM(
    vocab_size=len(word_to_idx),
    embedding_dim=embedding_dim,
    hidden_dim=hidden_dim,
    output_dim=output_dim,
    n_layers=n_layers,
    bidirectional=bidirectional,
    dropout=dropout
).to(device)

print(model)

criterion = nn.BCELoss()
optimizer = optim.Adam(model.parameters(), lr=learning_rate)
scheduler = optim.lr_scheduler.ReduceLROnPlateau(optimizer, mode='min', factor=0.5, patience=2)

train_losses = []
train_accuracies = []
val_accuracies = []

print("\nНачало обучения...")

for epoch in range(epochs):
    model.train()
    epoch_loss = 0.0
    correct = 0
    total = 0

    for texts, labels in train_loader:
        texts = texts.to(device)
        labels = labels.to(device).unsqueeze(1)

        outputs = model(texts)
        loss = criterion(outputs, labels)

        optimizer.zero_grad()
        loss.backward()
        torch.nn.utils.clip_grad_norm_(model.parameters(), max_norm=1.0)
        optimizer.step()

        epoch_loss += loss.item()

        predicted = (outputs > 0.5).long().squeeze()
        total += labels.size(0)
        correct += (predicted == labels.squeeze()).sum().item()

    avg_loss = epoch_loss / len(train_loader)
    accuracy = 100 * correct / total
    train_losses.append(avg_loss)
    train_accuracies.append(accuracy)

    model.eval()
    val_correct = 0
    val_total = 0
    with torch.no_grad():
        for texts, labels in test_loader:
            texts = texts.to(device)
            labels = labels.to(device).unsqueeze(1)

            outputs = model(texts)
            predicted = (outputs > 0.5).long().squeeze()
            val_total += labels.size(0)
            val_correct += (predicted == labels.squeeze()).sum().item()

    val_accuracy = 100 * val_correct / val_total
    val_accuracies.append(val_accuracy)
    scheduler.step(avg_loss)

    print(f'Epoch [{epoch+1}/{epochs}], Loss: {avg_loss:.4f}, Train Acc: {accuracy:.2f}%, Val Acc: {val_accuracy:.2f}%')

fig, axes = plt.subplots(1, 2, figsize=(14, 4))

axes[0].plot(train_losses, linewidth=2, label='Train Loss')
axes[0].set_xlabel('Epoch')
axes[0].set_ylabel('Loss')
axes[0].set_title('Training Loss')
axes[0].grid(True, alpha=0.3)
axes[0].legend()

axes[1].plot(train_accuracies, linewidth=2, label='Train Accuracy')
axes[1].plot(val_accuracies, linewidth=2, label='Val Accuracy')
axes[1].set_xlabel('Epoch')
axes[1].set_ylabel('Accuracy (%)')
axes[1].set_title('Training and Validation Accuracy')
axes[1].grid(True, alpha=0.3)
axes[1].legend()

plt.tight_layout()
plt.savefig('lab6_metrics.png')
print("Saved lab6_metrics.png")

def predict_sentiment(text, model, word_to_idx, device, max_len=100):
    model.eval()
    text = preprocess_text(text)
    seq = [word_to_idx.get(word, 1) for word in text.split()]

    if len(seq) < max_len:
        seq += [0] * (max_len - len(seq))
    else:
        seq = seq[:max_len]

    tensor = torch.LongTensor(seq).unsqueeze(0).to(device)

    with torch.no_grad():
        output = model(tensor)
        confidence = output.item()
        sentiment = "Positive" if confidence > 0.5 else "Negative"

    return sentiment, confidence

test_texts = [
    "This movie was absolutely amazing and fantastic",
    "Terrible film, waste of time and very boring",
    "Great acting and wonderful story",
    "Awful movie, horrible characters and bad plot"
]

print("\nSentiment Analysis Results:")
for text in test_texts:
    sentiment, confidence = predict_sentiment(text, model, word_to_idx, device)
    print(f"\nText: {text}")
    print(f"Sentiment: {sentiment} (Confidence: {confidence:.4f})")

print("\nLab 6 complete!")
