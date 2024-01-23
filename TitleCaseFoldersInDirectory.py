import os

def title_case_folders(directory_path):
    # Get the list of all folders in the directory
    folders = [folder for folder in os.listdir(directory_path) if os.path.isdir(os.path.join(directory_path, folder))]

    # Iterate through each folder and rename to title case
    for folder in folders:
        old_path = os.path.join(directory_path, folder)
        new_folder = folder.title()
        new_path = os.path.join(directory_path, new_folder)

        # Rename the folder
        os.rename(old_path, new_path)
        print(f'Renamed: {folder} to {new_folder}')

if __name__ == "__main__":
    # Replace 'your_directory_path' with the actual path of the directory you want to process
    directory_path = 'Z:\\tv'
    
    title_case_folders(directory_path)
