rem git log --shortstat --no-merges --pretty=format:%%ai%%x09(%%ce)%%n%%s%%x09%%b%%x09%%N > changelog.txt
rem git log --no-merges --pretty=format:"%%ai%%x09%%s%%N" > changelog.txt
git log --no-merges --since="2026-01-01" --until="2030-01-01" --pretty=format:"%%ai%%x09%%s%%N" > changelog.txt