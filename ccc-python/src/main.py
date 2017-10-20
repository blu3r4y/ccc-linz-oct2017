import argparse

# global arguments
ARGS = None


def main():
    pass


# main entry point
if __name__ == '__main__':
    parser = argparse.ArgumentParser(
        description="[ccc-python]",
        formatter_class=argparse.RawTextHelpFormatter)
    parser.add_argument('CSV_DATA', type=str)
    parser.add_argument('--csv_delimiter', type=str, default='|')

    ARGS = parser.parse_args()

    main()
