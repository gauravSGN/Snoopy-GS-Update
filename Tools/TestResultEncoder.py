#!/usr/bin/python

from collections import OrderedDict
import xml.etree.ElementTree as ET
import sys
import os

FILE_LIST = None


def get_file_list():
    file_list = {}
    path = os.path.abspath('.')

    for root, dirs, files in os.walk(path):
        for filename in files:
            full_path = os.path.join(root, filename)
            file_list[filename] = os.path.relpath(full_path, path)

    return file_list


def encode_test_results(root, result):
    result["name"] = "unitTest"
    result["attributes"] = { "version": 1 }

    return result


def find_or_create_test_suite(root, name):
    global FILE_LIST
    path = FILE_LIST["%s.cs" % name]

    for child in root["children"]:
        if child["name"] == "file" and child["attributes"]["path"] == path:
            return child

    child = {
        "name": "file",
        "attributes": { "path": path },
        "children": []
    }

    root["children"].append(child)

    return child


def encode_test_case(root, result):
    suite_name = root.attrib["name"].split(".")[0]
    suite = find_or_create_test_suite(result, suite_name)

    test_case = {
        "name": "testCase",
        "attributes": {
            "name": root.attrib["name"].split(".")[-1],
            "duration": int(float(root.attrib["time"]) * 1000.0),
        },
        "children": []
    }

    suite["children"].append(test_case)

    return result


ENCODE_MAP = {
    "test-results": encode_test_results,
    "test-case": encode_test_case,
}


def encode_results(root, result):
    if root.tag in ENCODE_MAP:
        result = ENCODE_MAP[root.tag](root, result)

    if "children" not in result:
        result["children"] = []

    for node in root:
        encode_results(node, result)

    return result


def write_results(f, result, indent=0):
    f.write("  " * indent)

    f.write("<%s" % result["name"])
    for k, v in result["attributes"].iteritems():
        f.write(' %s="%s"' % (k, str(v)))

    if len(result["children"]) == 0:
        f.write("/>\n")
    else:
        f.write(">\n")

        for child in result["children"]:
            write_results(f, child, indent + 1)

        f.write("  " * indent)
        f.write("</%s>\n" % result["name"])


if __name__ == "__main__":
    if len(sys.argv) != 3:
        print "Usage: %s [input_filename] [output_filename]" % sys.argv[0]
        sys.exit(1)

    FILE_LIST = get_file_list()

    input_filename, output_filename = map(os.path.abspath, sys.argv[1:3])

    print "Parsing input file: %s" % input_filename
    tree = ET.parse(input_filename)

    print "Converting results"
    result = encode_results(tree.getroot(), {})

    print "Writing output file: %s" % output_filename
    with open(output_filename, "w") as f:
        write_results(f, result)
