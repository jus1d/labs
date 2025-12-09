#include <iostream>
#include <string>
#include <vector>
#include <cstdlib>
#include <cstring>

using namespace std;

class Athlete {
private:
    string full_name;

public:
    Athlete(string name) {
        if (name.empty()) {
            throw "ФИО спортсмена не может быть пустым";
        }
        full_name = name;
        cout << "Создан спортсмен: " << full_name << endl;
    }

    ~Athlete() {
        cout << "Удален спортсмен: " << full_name << endl;
    }

    string get_name() const {
        return full_name;
    }
};

class SportType {
private:
    string name;

public:
    SportType(const string& sport_name) {
        if (sport_name.empty()) {
            throw "Название вида спорта не может быть пустым";
        }
        name = sport_name;
        cout << "Создан вид спорта: " << name << endl;
    }

    ~SportType() {
        cout << "Удален вид спорта: " << name << endl;
    }

    string get_name() const {
        return name;
    }

    static int find_sport_index(const vector<SportType*>& sports, const string& sport_name) {
        for (int i = 0; i < (int)sports.size(); i++) {
            if (sports[i]->get_name() == sport_name) {
                return i;
            }
        }
        return -1;
    }
};

class IndexRecord {
private:
    int athlete_index;
    int sport_index;

public:
    IndexRecord(int athlete_idx, int sport_idx) {
        if (athlete_idx < 0) {
            throw "Индекс спортсмена не может быть отрицательным!";
        }
        if (sport_idx < 0) {
            throw "Индекс вида спорта не может быть отрицательным!";
        }
        athlete_index = athlete_idx;
        sport_index = sport_idx;
        cout << "Создана индексная связь: спортсмен[" << athlete_index
            << "] - вид спорта[" << sport_index << "]" << endl;
    }

    ~IndexRecord() {
        cout << "Удалена индексная связь: спортсмен[" << athlete_index
            << "] - вид спорта[" << sport_index << "]" << endl;
    }

    int get_athlete_index() const {
        return athlete_index;
    }

    int get_sport_index() const {
        return sport_index;
    }

    void set_sport_index(int idx) {
        if (idx < 0) {
            throw "Индекс вида спорта не может быть отрицательным!";
        }
        sport_index = idx;
    }

    static vector<int> find_athletes_by_sport(const vector<IndexRecord*>& indexes, int sport_idx) {
        vector<int> athlete_indices;
        for (const auto& record : indexes) {
            if (record->get_sport_index() == sport_idx) {
                athlete_indices.push_back(record->get_athlete_index());
            }
        }
        return athlete_indices;
    }

    static vector<int> find_sports_by_athlete(const vector<IndexRecord*>& indexes, int athlete_idx) {
        vector<int> sport_indices;
        for (const auto& record : indexes) {
            if (record->get_athlete_index() == athlete_idx) {
                sport_indices.push_back(record->get_sport_index());
            }
        }
        return sport_indices;
    }
};

int compare_athletes(const void* a, const void* b) {
    Athlete** athlete_a = (Athlete**)a;
    Athlete** athlete_b = (Athlete**)b;
    return (*athlete_a)->get_name().compare((*athlete_b)->get_name());
}

void swap(void* a, void* b, size_t size) {
    char* temp = new char[size];
    memcpy(temp, a, size);
    memcpy(a, b, size);
    memcpy(b, temp, size);
    delete[] temp;
}

int partition(void* base, int low, int high, size_t size, int (*compar)(const void*, const void*)) {
    char* arr = (char*)base;
    void* pivot = arr + high * size;
    int i = low - 1;

    for (int j = low; j < high; j++) {
        if (compar(arr + j * size, pivot) <= 0) {
            i++;
            swap(arr + i * size, arr + j * size, size);
        }
    }
    swap(arr + (i + 1) * size, arr + high * size, size);
    return i + 1;
}

void quick_sort(void* base, int low, int high, size_t size, int (*compar)(const void*, const void*)) {
    if (low < high) {
        int pi = partition(base, low, high, size, compar);
        quick_sort(base, low, pi - 1, size, compar);
        quick_sort(base, pi + 1, high, size, compar);
    }
}

void my_qsort(void* base, size_t num, size_t size, int (*compar)(const void*, const void*)) {
    if (num <= 1) return;
    quick_sort(base, 0, (int)num - 1, size, compar);
}

int add_athlete(vector<Athlete*>& athletes, const string& name) {
    Athlete* new_athlete = new Athlete(name);
    athletes.push_back(new_athlete);
    return (int)athletes.size() - 1;
}

int add_sport_type(vector<SportType*>& sports, const string& name) {
    SportType* new_sport = new SportType(name);
    sports.push_back(new_sport);
    return (int)sports.size() - 1;
}

void add_index_record(vector<IndexRecord*>& indexes, int athlete_index, int sport_index,
    const vector<Athlete*>& athletes, const vector<SportType*>& sports) {
    if (athlete_index < 0 || athlete_index >= (int)athletes.size()) {
        throw "Спортсмен с таким индексом не найден!";
    }

    if (sport_index < 0 || sport_index >= (int)sports.size()) {
        throw "Вид спорта с таким индексом не найден!";
    }

    IndexRecord* new_record = new IndexRecord(athlete_index, sport_index);
    indexes.push_back(new_record);
}

void add_athlete_with_sport(vector<Athlete*>& athletes, vector<SportType*>& sports,
    vector<IndexRecord*>& indexes) {
    string athlete_name, sport_name;

    cout << "\nВведите ФИО спортсмена: ";
    cin.ignore();
    getline(cin, athlete_name);

    cout << "Введите вид спорта: ";
    getline(cin, sport_name);

    int athlete_idx = add_athlete(athletes, athlete_name);

    int sport_idx = SportType::find_sport_index(sports, sport_name);

    if (sport_idx == -1) {
        sport_idx = add_sport_type(sports, sport_name);
    }

    add_index_record(indexes, athlete_idx, sport_idx, athletes, sports);
    cout << "Спортсмен успешно добавлен" << endl;
}

void change_athlete_sport(vector<Athlete*>& athletes, vector<SportType*>& sports,
    vector<IndexRecord*>& indexes) {
    if (athletes.empty()) {
        cout << "Список спортсменов пуст" << endl;
        return;
    }

    cout << "\nСписок спортсменов:" << endl;
    for (int i = 0; i < (int)athletes.size(); i++) {
        cout << "[" << i << "] " << athletes[i]->get_name() << endl;
    }

    int athlete_idx;
    cout << "\nВведите индекс спортсмена: ";
    cin >> athlete_idx;

    if (athlete_idx < 0 || athlete_idx >= (int)athletes.size()) {
        throw "Неверный индекс спортсмена";
    }

    string new_sport_name;
    cout << "Введите новый вид спорта: ";
    cin.ignore();
    getline(cin, new_sport_name);

    int new_sport_idx = SportType::find_sport_index(sports, new_sport_name);

    if (new_sport_idx == -1) {
        new_sport_idx = add_sport_type(sports, new_sport_name);
    }

    bool found = false;
    for (auto& record : indexes) {
        if (record->get_athlete_index() == athlete_idx) {
            record->set_sport_index(new_sport_idx);
            found = true;
            break;
        }
    }

    if (!found) {
        add_index_record(indexes, athlete_idx, new_sport_idx, athletes, sports);
    }

    cout << "Вид спорта изменен" << endl;
}

void display_athletes_by_sport(const vector<Athlete*>& athletes,
    const vector<SportType*>& sports,
    const vector<IndexRecord*>& indexes) {
    if (sports.empty()) {
        cout << "Список видов спорта пуст" << endl;
        return;
    }

    string sport_name;
    cout << "\nВведите название вида спорта: ";
    cin.ignore();
    getline(cin, sport_name);

    int sport_idx = SportType::find_sport_index(sports, sport_name);

    if (sport_idx == -1) {
        cout << "Вид спорта не найден." << endl;
        return;
    }

    vector<int> athlete_indices = IndexRecord::find_athletes_by_sport(indexes, sport_idx);

    if (athlete_indices.empty()) {
        cout << "Нет спортсменов, занимающихся этим видом спорта." << endl;
        return;
    }

    for (int athlete_idx : athlete_indices) {
        if (athlete_idx < 0 || athlete_idx >= (int)athletes.size()) {
            throw "Неверный индекс спортсмена в записях";
        }
        cout << athletes[athlete_idx]->get_name() << endl;
    }
}

void display_all_athletes_sorted(const vector<Athlete*>& athletes, const vector<SportType*>& sports, const vector<IndexRecord*>& indexes) {
    cout << "\nВсе спортсмены:" << endl;

    if (athletes.empty()) {
        cout << "Список пуст." << endl;
        return;
    }

    vector<Athlete*> sorted_athletes = athletes;
    if (!sorted_athletes.empty()) {
        my_qsort(sorted_athletes.data(), sorted_athletes.size(), sizeof(Athlete*), compare_athletes);
    }

    for (auto& athlete : sorted_athletes) {
        if (!athlete) {
            throw "Обнаружен nullptr в списке спортсменов";
        }

        int athlete_idx = -1;
        for (int i = 0; i < (int)athletes.size(); i++) {
            if (athletes[i] == athlete) {
                athlete_idx = i;
                break;
            }
        }

        if (athlete_idx == -1) continue;

        cout << athlete->get_name() << " - ";

        vector<int> sport_indices = IndexRecord::find_sports_by_athlete(indexes, athlete_idx);

        if (sport_indices.empty()) {
            cout << "вид спорта не указан";
        }
        else {
            for (int sport_idx : sport_indices) {
                if (sport_idx < 0 || sport_idx >= (int)sports.size()) {
                    throw "Неверный индекс вида спорта";
                }
                if (!sports[sport_idx]) {
                    throw "Обнаружен nullptr в списке видов спорта";
                }
                cout << sports[sport_idx]->get_name() << " ";
            }
        }
        cout << endl;
    }
}

void display_athletes_by_sport_sorted(const vector<Athlete*>& athletes, const vector<SportType*>& sports, const vector<IndexRecord*>& indexes) {
    if (sports.empty()) {
        cout << "Список пуст!" << endl;
        return;
    }

    string sport_name;
    cout << "\nВведите название вида спорта: ";
    cin.ignore();
    getline(cin, sport_name);

    int sport_idx = SportType::find_sport_index(sports, sport_name);

    if (sport_idx == -1) {
        cout << "Вид спорта не найден." << endl;
        return;
    }

    vector<int> athlete_indices = IndexRecord::find_athletes_by_sport(indexes, sport_idx);

    if (athlete_indices.empty()) {
        cout << "Нет спортсменов, занимающихся этим видом спорта." << endl;
        return;
    }

    vector<Athlete*> sport_athletes;
    for (int athlete_idx : athlete_indices) {
        if (athlete_idx < 0 || athlete_idx >= (int)athletes.size()) {
            throw "Неверный индекс спортсмена в записях!";
        }
        sport_athletes.push_back(athletes[athlete_idx]);
    }

    if (!sport_athletes.empty()) {
        my_qsort(sport_athletes.data(), sport_athletes.size(), sizeof(Athlete*), compare_athletes);
    }

    cout << "\nСпортсмены, занимающиеся " << sport_name << ":" << endl;
    for (const auto athlete : sport_athletes) {
        if (!athlete) {
            throw "Обнаружен nullptr в списке спортсменов!";
        }
        cout << athlete->get_name() << endl;
    }
}

void cleanup(vector<Athlete*>& athletes, vector<SportType*>& sports, vector<IndexRecord*>& indexes) {
    for (auto athlete : athletes) delete athlete;
    for (auto sport : sports) delete sport;
    for (auto index : indexes) delete index;

    athletes.clear();
    sports.clear();
    indexes.clear();
}

int main() {
    vector<Athlete*> athletes;
    vector<SportType*> sports;
    vector<IndexRecord*> indexes;

    int choice;
    bool running = true;

    while (running) {
        cout << "\n1. Добавить спортсмена" << endl;
        cout << "2. Изменить вид спорта" << endl;
        cout << "3. Вывести спортсменов по виду спорта" << endl;
        cout << "4. Вывести всех спортсменов" << endl;
        cout << "5. Вывести спортсменов по виду спорта (сортировка)" << endl;
        cout << "0. Выход" << endl;
        cout << "Выберите: ";
        cin >> choice;

        try {
            switch (choice) {
            case 1:
                add_athlete_with_sport(athletes, sports, indexes);
                break;

            case 2:
                change_athlete_sport(athletes, sports, indexes);
                break;

            case 3:
                display_athletes_by_sport(athletes, sports, indexes);
                break;

            case 4:
                display_all_athletes_sorted(athletes, sports, indexes);
                break;

            case 5:
                display_athletes_by_sport_sorted(athletes, sports, indexes);
                break;

            case 0:
                running = false;
                break;

            default:
                cout << "Неверный выбор!" << endl;
            }
        }
        catch (const char* e) {
            cerr << "Ошибка: " << e << endl;
        }
        catch (...) {
            cerr << "Поймана неизвестная ошибка" << endl;
            return 1;
        }
    }

    cleanup(athletes, sports, indexes);
    cout << "Программа завершена." << endl;

    return 0;
}
