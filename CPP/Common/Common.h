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
    inline std::vector<std::string> split(const std::regex& re, const std::string& subject)
    {
        using namespace std;
        vector<string> container {
            sregex_token_iterator(subject.begin(), subject.end(), re, -1),
            sregex_token_iterator()
        };
        return container;
    }

    inline std::vector<int> split_numbers(const std::string& line, bool signedNumbers=false)
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
}

#endif //CPP_COMMON_H
