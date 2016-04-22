local lib = require "TestLuaLib.cs";

local tmp1 = 1;

local function testCallLua()
print("testCallLua success!"..tmp1);
tmp1 = tmp1 + 1;
end

local function testCallLuaWithArgs(boolArg,intArg,strArg)
if boolArg == false then
print("fuck");
end
print("boolArg : "..boolArg);
print("intArg+1 : "..(intArg+1));
print("strArg : "..strArg.." !");
return { a = 1,b=2};
end

local function testTable(tableArg)
	--print("table.n + 1 = "..(tableArg.n+1))
	local sum = 0;
	for i=1,96 do
		sum = sum + tableArg[i];
		--print("array["..i.."] = "..tableArg[i])
	end
	return sum;
end

return 
{
    testCallLua = testCallLua,
    testCallLuaWithArgs = testCallLuaWithArgs,
	testTable = testTable,
}