# python can use C# objects
# use a global object passed from C# to print into the message box

# directly using the C# object
cm.MessageWriteLine("Hello from Python")

# define a convenience method using ComponentManager.MessageWriteLine
def message(*arg):
  asString = '  '.join(str(i) for i in arg)
  cm.MessageWriteLine(asString)

# using convenience, useful due to parameter types
message("Hello number:", 4711)

# message can output also variable lists
message("Show a range", range(3, 11))

# output can be redirected to any object which implement method write and property softspace
import sys
sys.stdout=cmd
print "Redirected", 4711, range(50, 60)



