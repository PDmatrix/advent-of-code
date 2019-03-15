//
// Created by dmatrix on 1/12/19.
//

#ifndef CPP_COMMON_H
#define CPP_COMMON_H

#include <algorithm>
#include <fstream>
#include <regex>
#include <string>
#include <vector>

namespace AdventOfCode {
    static inline std::vector<std::string> split(const std::regex& re, const std::string& subject)
    {
        using namespace std;
        vector<string> container {
            sregex_token_iterator(subject.begin(), subject.end(), re, -1),
            sregex_token_iterator()
        };
        return container;
    }

    static inline std::vector<int> split_numbers(const std::string& line, bool signedNumbers=false)
    {
        using namespace std;
        vector<int> ints;
        regex re(signedNumbers ? "[^-\\d]+" : "\\D+");
        transform(sregex_token_iterator(line.begin(), line.end(), re, -1), sregex_token_iterator(),
                  back_inserter(ints), [](const auto& s) -> int {
                    try
                    {
                        return stoi(s);
                    } catch(const invalid_argument&)
                    {
                        return 0;
                    }
                });
        return ints;
    }

    // trim from start (in place)
    static inline void ltrim(std::string &s) {
        s.erase(s.begin(), std::find_if(s.begin(), s.end(), [](int ch) {
            return !std::isspace(ch);
        }));
    }

    // trim from end (in place)
    static inline void rtrim(std::string &s) {
        s.erase(std::find_if(s.rbegin(), s.rend(), [](int ch) {
            return !std::isspace(ch);
        }).base(), s.end());
    }

    // trim from both ends (in place)
    static inline void trim(std::string &s) {
        ltrim(s);
        rtrim(s);
    }

    // trim from start (copying)
    static inline std::string ltrim_copy(std::string s) {
        ltrim(s);
        return s;
    }

    // trim from end (copying)
    static inline std::string rtrim_copy(std::string s) {
        rtrim(s);
        return s;
    }

    // trim from both ends (copying)
    static inline std::string trim_copy(std::string s) {
        trim(s);
        return s;
    }
}

#endif //CPP_COMMON_H
