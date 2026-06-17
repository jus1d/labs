#!/usr/bin/env python3

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import load_wine
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import accuracy_score, precision_score, recall_score, f1_score, confusion_matrix, classification_report
import seaborn as sns
import warnings
warnings.filterwarnings('ignore')

print("=" * 80)
print("Lab 7: AutoML with H2O and AutoKeras")
print("=" * 80)

dataset_name = "Wine Classification"
print(f"\nDataset: {dataset_name}")

wine = load_wine()
X = pd.DataFrame(wine.data, columns=wine.feature_names)
y = pd.Series(wine.target, name='target')

print(f"Features: {X.shape[1]}")
print(f"Samples: {X.shape[0]}")
print(f"Classes: {len(np.unique(y))}")
print(f"\nClass distribution:\n{y.value_counts()}")

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42, stratify=y)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

X_train_scaled = pd.DataFrame(X_train_scaled, columns=X.columns)
X_test_scaled = pd.DataFrame(X_test_scaled, columns=X.columns)

print(f"\nTrain set: {X_train_scaled.shape[0]}")
print(f"Test set: {X_test_scaled.shape[0]}")

results = {}

try:
    print("\n" + "=" * 80)
    print("H2O AutoML")
    print("=" * 80)

    import h2o
    from h2o.automl import H2OAutoML

    h2o.init(strict_version_check=False, ignore_config=True)

    train_h2o = h2o.H2OFrame(pd.concat([X_train_scaled, y_train.reset_index(drop=True)], axis=1))
    test_h2o = h2o.H2OFrame(pd.concat([X_test_scaled, y_test.reset_index(drop=True)], axis=1))

    aml = H2OAutoML(
        max_models=10,
        seed=42,
        max_runtime_secs=60,
        verbosity='info'
    )

    aml.train(
        x=X.columns.tolist(),
        y='target',
        training_frame=train_h2o
    )

    print(f"\nH2O Best Model: {aml.leader.model_id}")

    h2o_pred = aml.predict(test_h2o)
    h2o_pred_df = h2o_pred.as_data_frame()
    h2o_pred_np = h2o_pred_df['predict'].values.astype(int)

    h2o_accuracy = accuracy_score(y_test, h2o_pred_np)
    h2o_precision = precision_score(y_test, h2o_pred_np, average='weighted', zero_division=0)
    h2o_recall = recall_score(y_test, h2o_pred_np, average='weighted', zero_division=0)
    h2o_f1 = f1_score(y_test, h2o_pred_np, average='weighted', zero_division=0)

    results['H2O AutoML'] = {
        'accuracy': h2o_accuracy,
        'precision': h2o_precision,
        'recall': h2o_recall,
        'f1': h2o_f1,
        'predictions': h2o_pred_np
    }

    print(f"Accuracy:  {h2o_accuracy:.4f}")
    print(f"Precision: {h2o_precision:.4f}")
    print(f"Recall:    {h2o_recall:.4f}")
    print(f"F1-Score:  {h2o_f1:.4f}")

    h2o.cluster().shutdown()

except Exception as e:
    print(f"H2O AutoML error: {e}")
    print("Continuing with AutoKeras...")

try:
    print("\n" + "=" * 80)
    print("Neural Network (Keras Sequential)")
    print("=" * 80)

    import tensorflow as tf
    from tensorflow import keras
    from tensorflow.keras import layers

    print("\nTraining neural network model...")

    model = keras.Sequential([
        layers.Dense(64, activation='relu', input_shape=(X_train_scaled.shape[1],)),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])

    model.compile(
        optimizer='adam',
        loss='sparse_categorical_crossentropy',
        metrics=['accuracy']
    )

    model.fit(
        X_train_scaled.values,
        y_train.values,
        validation_split=0.2,
        epochs=100,
        batch_size=16,
        verbose=0
    )

    ak_pred_proba = model.predict(X_test_scaled.values, verbose=0)
    ak_pred = np.argmax(ak_pred_proba, axis=1)

    ak_accuracy = accuracy_score(y_test, ak_pred)
    ak_precision = precision_score(y_test, ak_pred, average='weighted', zero_division=0)
    ak_recall = recall_score(y_test, ak_pred, average='weighted', zero_division=0)
    ak_f1 = f1_score(y_test, ak_pred, average='weighted', zero_division=0)

    results['Keras NN'] = {
        'accuracy': ak_accuracy,
        'precision': ak_precision,
        'recall': ak_recall,
        'f1': ak_f1,
        'predictions': ak_pred
    }

    print(f"Accuracy:  {ak_accuracy:.4f}")
    print(f"Precision: {ak_precision:.4f}")
    print(f"Recall:    {ak_recall:.4f}")
    print(f"F1-Score:  {ak_f1:.4f}")

except Exception as e:
    print(f"Keras NN error: {e}")
    print("Install: pip install tensorflow")

if not results:
    print("\nError: No models trained successfully")
    exit(1)

print("\n" + "=" * 80)
print("Results Comparison")
print("=" * 80)

comparison_df = pd.DataFrame(results).T
print(f"\n{comparison_df}")

fig, axes = plt.subplots(2, 2, figsize=(12, 10))

metrics = ['accuracy', 'precision', 'recall', 'f1']
colors = ['#1f77b4', '#ff7f0e', '#2ca02c']

ax = axes[0, 0]
for idx, (model_name, scores) in enumerate(results.items()):
    values = [scores[m] for m in metrics]
    ax.bar([i + idx * 0.35 for i in range(len(metrics))], values, width=0.35, label=model_name, color=colors[idx])

ax.set_ylabel('Score')
ax.set_title('Metrics Comparison')
ax.set_xticks([i + 0.175 for i in range(len(metrics))])
ax.set_xticklabels(metrics)
ax.legend()
ax.grid(axis='y', alpha=0.3)

ax = axes[0, 1]
model_names = list(results.keys())
accuracies = [results[m]['accuracy'] for m in model_names]
ax.barh(model_names, accuracies, color=colors[:len(model_names)])
ax.set_xlabel('Accuracy')
ax.set_title('Accuracy Comparison')
ax.set_xlim([0, 1])
for i, v in enumerate(accuracies):
    ax.text(v + 0.01, i, f'{v:.4f}', va='center')
ax.grid(axis='x', alpha=0.3)

ax = axes[1, 0]
x_pos = np.arange(len(model_names))
width = 0.2
for idx, metric in enumerate(metrics):
    values = [results[m][metric] for m in model_names]
    ax.bar(x_pos + idx * width, values, width, label=metric, color=colors)

ax.set_ylabel('Score')
ax.set_title('All Metrics by Model')
ax.set_xticks(x_pos + width * 1.5)
ax.set_xticklabels(model_names)
ax.legend()
ax.set_ylim([0, 1.1])
ax.grid(axis='y', alpha=0.3)

ax = axes[1, 1]
ax.axis('off')
summary_text = "Summary:\n\n"
for model_name, scores in results.items():
    summary_text += f"{model_name}:\n"
    summary_text += f"  Accuracy: {scores['accuracy']:.4f}\n"
    summary_text += f"  F1-Score: {scores['f1']:.4f}\n\n"

ax.text(0.1, 0.9, summary_text, transform=ax.transAxes, fontsize=10,
        verticalalignment='top', fontfamily='monospace',
        bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))

plt.tight_layout()
plt.savefig('lab7_comparison.png', dpi=100, bbox_inches='tight')
print("\nSaved lab7_comparison.png")

print("\n" + "=" * 80)
print("Best Model:")
best_model = max(results.items(), key=lambda x: x[1]['accuracy'])
print(f"{best_model[0]} with accuracy: {best_model[1]['accuracy']:.4f}")
print("=" * 80)

print("\nLab 7 complete!")
