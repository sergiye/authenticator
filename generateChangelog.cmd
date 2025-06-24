rem git log --shortstat --no-merges --pretty=format:%%ai%%x09(%%ce)%%n%%s%%x09%%b%%x09%%N > changelog.txt
git log --no-merges --pretty=format:"%%ai%%x09%%s%%N" > changelog.txt
