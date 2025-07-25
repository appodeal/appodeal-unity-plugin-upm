#!/usr/bin/env python3
# validate_min_sdk.py
# Parses an XML file with <iosPod> entries, fetches .podspec.json from CocoaPods CDN
# or custom GitHub repos, compares minTargetSdk to podspec iOS deployment target,
# and outputs a Markdown report. Exits with code 1 on mismatches or missing specs.

import sys
import xml.etree.ElementTree as ET
import hashlib
from concurrent.futures import ThreadPoolExecutor, as_completed

import requests
from requests.exceptions import RequestException

# HTTP session for connection pooling
session = requests.Session()
TIMEOUT = 10  # seconds
MAX_WORKERS = 5  # parallel threads


def _fetch_json(url):
    try:
        r = session.get(url, timeout=TIMEOUT)
        if r.status_code == 200:
            try:
                return r.json()
            except ValueError:
                # invalid JSON, treat as missing
                return None
    except RequestException:
        # network or other error, treat as missing
        return None
    return None


def fetch_podspec_from_cdn(name, version):
    digest = hashlib.md5(name.encode('utf-8')).hexdigest()
    d1, d2, d3 = digest[0], digest[1], digest[2]
    url = (
        f"https://cdn.cocoapods.org/specs/"
        f"{d1}/{d2}/{d3}/{name}/{version}/{name}.podspec.json"
    )
    data = _fetch_json(url)
    return data, url


def fetch_podspec_from_custom(owner, repo, name, version):
    url = f"https://raw.githubusercontent.com/{owner}/{repo}/master/{name}/{version}/{name}.podspec.json"
    data = _fetch_json(url)
    return data, url


def extract_ios_sdk(podspec):
    if not podspec:
        return None
    for key in ('platforms', 'sdk'):
        val = podspec.get(key)
        if isinstance(val, dict):
            return val.get('ios')
    return None


def process_pod(entry, sources):
    name, version, xml_sdk = entry['name'], entry['version'], entry['xml_sdk']
    podspec, url = fetch_podspec_from_cdn(name, version)
    found = bool(podspec)
    if not found:
        for owner, repo in sources:
            podspec, url = fetch_podspec_from_custom(owner, repo, name, version)
            if podspec:
                found = True
                break
    ios_sdk = extract_ios_sdk(podspec)
    match = found and ios_sdk == xml_sdk
    return {**entry, 'ios_sdk': ios_sdk, 'url': url, 'found': found, 'match': match}


def main(xml_path):
    # validate and parse XML
    try:
        tree = ET.parse(xml_path)
    except ET.ParseError as e:
        print(f"Invalid XML: {e}")
        sys.exit(1)
    root = tree.getroot()

    # gather GitHub sources
    sources = []
    for s in root.findall('.//sources/source'):
        src = s.text.strip()
        if 'github.com' in src:
            parts = src.rstrip('.git').split('/')
            sources.append((parts[-2], parts[-1]))

    # collect pod entries
    pods = [
        {'name': pod.get('name'), 'version': pod.get('version'), 'xml_sdk': pod.get('minTargetSdk')}
        for pod in root.findall('.//iosPod')
    ]

    # fetch specs in parallel
    with ThreadPoolExecutor(max_workers=MAX_WORKERS) as executor:
        futures = [executor.submit(process_pod, p, sources) for p in pods]
        results = [f.result() for f in as_completed(futures)]

    mismatches = [r for r in results if r['found'] and not r['match']]
    not_found = [r for r in results if not r['found']]
    matches = [r for r in results if r['match']]

    # build Markdown report
    md = []

    md.append('### 🚨 SDK Mismatches')
    if mismatches:
        md.append(f"- **{len(mismatches)} mismatches found.**")
        md.append('<details open>')
        md.append('')
        md.append('| Pod | XML SDK | JSON SDK |')
        md.append('| --- | ------- | -------- |')
        for r in mismatches:
            md.append(f"| {r['name']} | {r['xml_sdk']} | [{r['ios_sdk']}]({r['url']}) |")
        md.append('')
        md.append('</details>')
    else:
        md.append('- 0 mismatches ✅')
    md.append('')

    md.append('### ⚠️ Podspecs Not Found')
    if not_found:
        md.append(f"- **{len(not_found)} podspecs missing.**")
        md.append('<details open>')
        md.append('')
        md.append('| Pod | XML SDK | JSON SDK |')
        md.append('| --- | ------- | -------- |')
        for r in not_found:
            md.append(f"| {r['name']} | {r['xml_sdk']} | Not Found |")
        md.append('')
        md.append('</details>')
    else:
        md.append('- All podspecs found ✅')
    md.append('')

    md.append('### ✅ Matches')
    if matches:
        md.append(f"- **{len(matches)} matches found.** 💪")
        md.append('<details>')
        md.append('')
        md.append('| Pod | XML SDK | JSON SDK |')
        md.append('| --- | ------- | -------- |')
        for r in matches:
            md.append(f"| {r['name']} | {r['xml_sdk']} | [{r['ios_sdk']}]({r['url']}) |")
        md.append('')
        md.append('</details>')
    else:
        md.append('- No matches ❌')
    md.append('')

    report = '\n'.join(md)
    print(report)
    with open('pod_sdk_report.md', 'w') as f:
        f.write(report)

    if mismatches or not_found:
        sys.exit(1)

if __name__ == '__main__':
    if len(sys.argv) != 2:
        print('Usage: validate_min_sdk.py <path-to-xml-or-txt>')
        sys.exit(1)
    main(sys.argv[1])
