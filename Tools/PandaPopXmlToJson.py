#!/usr/bin/python

from collections import OrderedDict
import xml.etree.ElementTree as ET
from json import dumps
import sys
import os


LEVEL_ATTRIBS = [
    ("remainingBubble", "shotCount", int),
]

BUBBLE_ATTRIBS = [
    ("typeID", "type", int),
    ("normalBubbleSubType", "contentType", int),
    ("grid_x", "x", int),
    ("grid_y", "y", int),
]


def get_json_filename(args, xml_filename):
    if (len(args) > 2):
        return args[2]

    return os.path.splitext(xml_filename)[0] + ".json"


def copy_attribs(attrib_list, source, destination):
    for k, v, f in attrib_list:
        if k in source:
            destination[v] = f(source[k])


def get_level_structure(xml_root):
    level = OrderedDict()

    copy_attribs(LEVEL_ATTRIBS, xml_root.attrib, level)

    level["powerUpFills"] = get_lantern_fills(xml_root)
    level["bubbles"] = list(map(parse_bubble, xml_root))
    adjust_y_coordinates(level["bubbles"])
    fix_top_row(level["bubbles"])

    return level


def get_lantern_fills(level):
    return [
        level.attrib["bombFill"],
        level.attrib["horzFill"],
        level.attrib["snakeFill"],
        level.attrib["fireFill"],
    ]


def parse_bubble(bubble_data):
    bubble = OrderedDict()

    copy_attribs(BUBBLE_ATTRIBS, bubble_data.attrib, bubble)

    return bubble


def adjust_y_coordinates(bubbles):
    max_y = reduce(lambda a, b: max(a, b["y"]), bubbles, 1)
    max_y += (max_y & 1)

    for bubble in bubbles:
        bubble["y"] = max_y - bubble["y"]


def fix_top_row(bubbles):
    top_row = [b for b in bubbles if b["y"] == 0]

    if len(top_row) == 0:
        next_row = [b for b in bubbles if b["y"] == 1]

        for bubble in next_row:
            new_bubble = OrderedDict()

            for k, v in bubble.iteritems():
                new_bubble[k] = v

            new_bubble["y"] = 0
            bubbles.append(new_bubble)


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print "Usage: %s xml_filename [json_filename]" % sys.argv[0]
        sys.exit(1)

    xml_filename = sys.argv[1]
    json_filename = get_json_filename(sys.argv, xml_filename)

    xml_filename, json_filename = map(os.path.abspath, (xml_filename, json_filename))

    print "Parsing %s..." % xml_filename
    tree = ET.parse(xml_filename)

    json_level = get_level_structure(tree.getroot())

    print "Writing %s..." % json_filename
    with open(json_filename, "w") as f:
        f.write(dumps(json_level, separators=(',', ':')))
