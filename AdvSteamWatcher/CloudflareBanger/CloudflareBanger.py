import cloudscraper
import sys

args = sys.argv
args_len = len(sys.argv)
args_len = args_len - 1
if args_len != 1:
    print("[ERROR] Incorrect arguments amount (expected 1, got %d)" % args_len)
    exit(0)

targetSite = args[1]

try:
    scraper = cloudscraper.create_scraper()
    print(scraper.get(targetSite).content)
except Exception as e:
    print("[ERROR] %s" % str(e))
