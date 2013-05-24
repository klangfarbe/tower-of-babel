#!/usr/bin/python
import sys
import os
import string
import re
import argparse
import json

def save_json_file(filename, data):
	if filename:
		f = open(filename, 'w')
		json.dump(data, f, sort_keys=True, indent=2, ensure_ascii=True)
		f.close()

# ------------------------------------------------------------------------------

def new_tower(id):
	return {
		'id': id,
		'version': 1,
		'author': None,
		'title': None,
		'behaviour': {
			'cameras': True,
			'destroysfloor': False,
			'timebombspeed': 0
		},
		'conditions': {
			'klondikes': None,
			'robots': None,
			'timelimit': None
		},
		'elements': {
			'offset': { 'x': None, 'y': None }
		},
		'fx': {
			'pattern': None,
			'patterncolor1': None,
			'patterncolor2': None,
			'groundcolor1': None,
			'groundcolor2': None
		}
	}

# ------------------------------------------------------------------------------

def process_meta(tower, match):
	if not match:
		return
	val = match.group('val')
	if val.startswith('true') or val.startswith('false'):
		val = bool(val)
	elif not val.startswith('0x'):
		val = int(val)
	tower[match.group('meta')][match.group('key')] = val

# ------------------------------------------------------------------------------

def process_elements(tower, match):
	if not match:
		return

# ------------------------------------------------------------------------------

def process_offset(tower, match):
	if not match:
		return
	tower['elements']['offset'][match.group('axis')] = int(match.group('offset'))

# ------------------------------------------------------------------------------

def process_levels(filename):
	# the regular expressions
	regex_tower = re.compile(r'tower\.(?P<id>\d{3})')
	regex_header = re.compile(r'header\.(?P<key>\w+)=(?P<val>.*)$')
	regex_elements = re.compile(r'elements\.\d(?P<floor>\d)\.(?P<row>\d\d)=(?P<elements>.*)$')
	regex_elements_offset = re.compile(r'offset(?P<axis>x|y)=(?P<offset>\d)$')
	regex_meta = re.compile(r'meta\.(?P<meta>behaviour|conditions|fx)\.(?P<key>\w+)=(?P<val>.*)$')

	tower = new_tower('000')

	f = open(filename, 'r')
	for line in f:
		match = regex_tower.match(line)
		if match and match.group('id') != tower['id']:
			save_json_file(tower['id'] + '.json', tower)
			tower = new_tower(match.group('id'))

		match = regex_header.search(line)
		if match:
			tower[match.group('key')] = match.group('val')

		process_offset(tower, regex_elements_offset.search(line))
		process_meta(tower, regex_meta.search(line))
		process_elements(tower, regex_elements.search(line))


# ------------------------------------------------------------------------------
# main
# ------------------------------------------------------------------------------
if __name__ == "__main__":
	parser = argparse.ArgumentParser(description = 'Converts level.properties file to separate json files')
	parser.add_argument('-f', '--file', metavar='FILE', help='The properties file', required=True)
	args = parser.parse_args()

	process_levels(args.file)