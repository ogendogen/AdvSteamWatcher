import cloudscraper
import sys

args = sys.argv
args_len = len(sys.argv)
args_len = args_len - 1
if args_len != 1:
    exit("[ERROR] Incorrect arguments amount (expected 1, got %d)" % args_len)

targetSite = args[1]

try:
    scraper = cloudscraper.create_scraper()
    exit(scraper.get(targetSite).content)
except Exception as e:
    exit("[ERROR] %s" % str(e))
