#!/usr/bin/python

from collections import OrderedDict
import json
import sys
import os


def convert_file(path):
    with open(path, 'r+') as file:
        data = json.loads(file.read(), object_pairs_hook = OrderedDict)

        if "bubbles" in data:
            print "-> Converting %s" % path

            data["puzzles"] = [{"bubbles": data["bubbles"]}]
            del data["bubbles"]

            file.seek(0)
            file.write(json.dumps(data))
            file.truncate()


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print "Usage: %s path_to_level_directory" % sys.argv[0]
        sys.exit(1)

    directory_with_levels = sys.argv[1]

    for root, dirs, files in os.walk(directory_with_levels):
        for file in files:
            extensions = file.split(".")

            if "json" in extensions and "meta" not in extensions:
                convert_file(os.path.join(root, file))

    print "Conversions complete"
